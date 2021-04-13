using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;
using BespokeFusion;

namespace ex1.Model
{
    // class for implementing the IFlightControl interface
    class FlightControl : IFlightControl
    {
        // playback speed
        private volatile float speed;
        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                this.speed = value;
                PropertyChangedNotify("Speed");
            }
        }

        // current timestep
        private volatile int timestep;
        public int Timestep
        {
            get
            {
                return timestep;
            }
            set
            {
                this.timestep = value;
                // notify timestep (and minute) has changed
                PropertyChangedNotify("Timestep");
                PropertyChangedNotify("Minute");

                if (Timestep == 0)
                {
                    // update length of slider and maximum (only do it at the beginning)
                    PropertyChangedNotify("Length");
                    PropertyChangedNotify("MaxTime");
                }

                if (Timestep != NumLines)
                {
                    // notify all properties that should update when timestep changes
                    PropertyChangedNotify("Throttle");
                    PropertyChangedNotify("Rudder");
                    PropertyChangedNotify("Aileron");
                    PropertyChangedNotify("Elevator");
                    PropertyChangedNotify("Altimeter");
                    PropertyChangedNotify("Airspeed");
                    PropertyChangedNotify("SpeedToAngle");
                    PropertyChangedNotify("HeadingDeg");
                    PropertyChangedNotify("PitchDeg");
                    PropertyChangedNotify("RollDeg");
                    PropertyChangedNotify("SideSlipDeg");
                    PropertyChangedNotify("FeaturePoints");
                    PropertyChangedNotify("SecondFeaturePoints");
                    PropertyChangedNotify("CorrFeaturesPoints");
                    PropertyChangedNotify("AnomalousPoints");
                }
            }
        }

        // number of lines in flight file
        private int numLines;
        public int NumLines
        {
            get
            {
                return numLines;
            }
            set
            {
                this.numLines = value;
            }
        }

        // is flight played in reverse
        private volatile bool isReverse;
        public bool IsReverse
        {
            get
            {
                return isReverse;
            }
            set
            {
                this.isReverse = value;
            }
        }

        // is the flight currently playing
        private volatile bool stop;
        public bool Stop
        {
            get { return stop; }
            set
            {
                stop = value;
                // update play icon if changed
                PropertyChangedNotify("PlayIcon");
            }
        }

        // destination port of server
        public int DestPort
        {
            get
            {
                return pilot.DestPort;
            }
            set
            {
                pilot.DestPort = value;
            }
        }

        // threshold for correlation
        public float Threshold
        {
            get
            {
                return research.CorrThreshold;
            }
            set
            {
                research.CorrThreshold = value;
            }
        }

        // paths of files
        private PathInfo paths;
        public PathInfo Paths
        {
            get { return paths; }
        }

        // other parts of the model
        private Pilot pilot;       
        private IFlightData flightdata;
        private IResearch research;

        // task for running in parallel to the GUI
        Task task;

        // implementing the PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        // constructor
        public FlightControl()
        {
            this.speed = 1;
            this.timestep = 0;
            this.numLines = 0;
            this.isReverse = false;
            this.stop = true;
            this.pilot = new SimplePilot();
            this.flightdata = new FlightData();
            this.research = new Research();
            this.paths = new PathInfo();
        }

        // start playing the flight and updating current timestep
        public void start()
        {
            Stop = false;
            this.task = new Task(delegate ()
            {
                // while flight is playing
                while (!stop)
                {
                    // send data of current timestep to server
                    if(!pilot.sendCurrentData(Timestep))
                    {
                        // if sending the data failed, stop the flight, show error and stop
                        Stop = true;
                        MaterialMessageBox.ShowError("disconnected from server. Please reconnect via settings.");
                        break;
                    }
                    // stop playing if reached the limit
                    if ((Timestep == 0 && IsReverse) || (Timestep >= numLines - 1 && !IsReverse))
                    {
                        Stop = true;
                        break;
                    }
                    // update to next timestep
                    Timestep += (IsReverse ? -1 : 1);

                    // sleep according to playback speed
                    Thread.Sleep((int)(100f / Math.Abs(Speed)));
                }
            });
            task.Start();
        }

        // stop playing the flight
        void IFlightControl.stop()
        {
            Stop = true;
        }

        // load features names
        public void loadFeatures(string xmlPath)
        {
            // load the XML file
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList xmlNode = doc.GetElementsByTagName("output");
            // cut the duplicates
            doc.LoadXml(xmlNode[0].OuterXml);
            // get feature names
            XmlNodeList xmlFeatures = doc.GetElementsByTagName("name");
            int i = 0;
            // iterate the feature names
            foreach (XmlNode feature in xmlFeatures)
            {
                string featureName = feature.InnerText;
                // if same feature name appears twice append 2 to its name
                if (flightdata.containsFeature(featureName))
                {
                    featureName += "2";
                }
                // add feature name to flightdata and research
                flightdata.addFeature(featureName, i);
                research.addFeature(featureName);
                i++;
            }
        }

        // load data from CSV files
        public void loadData(string csvPath)
        {
            // open CSV file
            System.IO.StreamReader f = new System.IO.StreamReader(csvPath);
            string line;
            int i = 0;
            // read each line
            while ((line = f.ReadLine()) != null)
            {
                // add line to pilot
                pilot.addLine(line);

                List<float> row = new List<float>();
                // split line by commas
                string[] values = line.Split(',');
                int j = 0;
                // add each value to research and flightdata
                foreach (string s in values)
                {
                    float val = float.Parse(s);
                    row.Add(val);
                    research.addData(j, val);
                    j++;
                }
                flightdata.addData(row);
                i++;
            }
            NumLines = i;
            f.Close();
        }

        // return value of feature in current timestep
        public float getCurrentData(String feature)
        {
            return flightdata.getValue(feature, timestep);
        }

        // send data of current timestep to FlightGear
        public bool SendCurrentData()
        {
            return pilot.sendCurrentData(Timestep);
        }

        // start the client and connect to server
        public bool startClient()
        {
            return pilot.startClient();
        }

        // end the client connection with server
        public void endClient()
        {
            pilot.endClient();
        }

        // analyze data from CSV files and detect anomalies
        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath)
        {
            research.analyzeData(normalFlightPath, newFlightPath, anomalyDetPath);
        }

        // reset all data loaded from files
        public void reset()
        {
            flightdata.reset();
            pilot.reset();
            research.reset();
            Timestep = 0;
        }

        // get name of correlative feature
        public String getCorrelative(String featureName)
        {
            return research.getCorrelative(featureName);
        }

        // return the features list
        public List<String> getFeaturesList()
        {
            return research.getFeaturesList();
        }

        // return list of timesteps with anomalies
        public List<int> getAnomaliesList(String featureName)
        {
            return research.getAnomaliesList(featureName);
        }

        // return list of points for given feature
        public List<DataPoint> getDataPoints(String featureName)
        {
            return research.getDataPoints(Timestep, featureName);
        }

        // return list of correlation points for given feature from recent 30 seconds
        public List<ScatterPoint> getRecentScatterPoints(String featureName)
        {
            return research.getRecentScatterPoints(Timestep, featureName);
        }

        // return list of correlation points for given feature which are anomalous from recent 30 seconds
        public List<ScatterPoint> getRecentAnomalousPoints(String featureName)
        {
            return research.getRecentAnomalousPoints(Timestep, featureName);
        }

        // get the annotation of given feature
        public Annotation getFeatureAnnotation(String featureName)
        {
            return research.getFeatureAnnotation(featureName);
        }

        // return minimal/maximal x/y values of given feature's correlation points
        public double getMinX(String featureName)
        {
            return research.getMinX(featureName);
        }
        public double getMaxX(String featureName)
        {
            return research.getMaxX(featureName);
        }
        public double getMinY(String featureName)
        {
            return research.getMinY(featureName);
        }
        public double getMaxY(String featureName)
        {
            return research.getMaxY(featureName);
        }

        // update when a property has changed
        public void PropertyChangedNotify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
