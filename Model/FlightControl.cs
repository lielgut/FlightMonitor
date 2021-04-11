using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Xml;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;
using BespokeFusion;

namespace ex1.Model
{
    class FlightControl : IFlightControl
    {
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
                PropertyChangedNotify("Timestep");
                PropertyChangedNotify("Minute");
                if (Timestep == 0)
                {
                    PropertyChangedNotify("Length");
                    PropertyChangedNotify("MaxTime");
                }

                if (Timestep != NumLines)
                {
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

        private volatile bool stop;
        public bool Stop
        {
            get { return stop; }
            set
            {
                stop = value;
                /*if(stop)
                {
                    thread.Join(); 
                }*/
                PropertyChangedNotify("PlayIcon");
            }
        }

        private PathInfo paths;
        public PathInfo Paths
        {
            get { return paths; }
        }

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

        private Pilot pilot;
        private IFlightData flightdata;
        private IResearch research;
        Thread thread;
        public event PropertyChangedEventHandler PropertyChanged;

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


        public void start()
        {
            Stop = false;
            this.thread = new Thread(delegate ()
            {
                while (!stop)
                {
                    if(!pilot.sendCurrentData(Timestep))
                    {
                        Stop = true;
                        MaterialMessageBox.ShowError("disconnected from server. Please reconnect via settings.");
                        break;
                    }
                    if ((Timestep == 0 && IsReverse) || (Timestep >= numLines - 1 && !IsReverse))
                    {
                        Stop = true;
                        break;
                    }
                    Timestep += (IsReverse ? -1 : 1);
                    Thread.Sleep((int)(100f / Math.Abs(Speed)));
                }
            });
            thread.Start();
        }

        void IFlightControl.stop()
        {
            Stop = true;
        }
        public void PropertyChangedNotify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        public void loadFeatures(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList xmlNode = doc.GetElementsByTagName("output");
            // cut the duplicates
            doc.LoadXml(xmlNode[0].OuterXml);
            // get feature names
            XmlNodeList xmlFeatures = doc.GetElementsByTagName("name");
            int i = 0;
            foreach (XmlNode feature in xmlFeatures)
            {
                string featureName = feature.InnerText;
                // if same feature name appears twice append 2 to its name
                if (flightdata.containsFeature(featureName))
                {
                    featureName += "2";
                }
                flightdata.addFeature(featureName, i);
                research.addFeature(featureName);
                i++;
            }
        }

        public void loadData(string csvPath)
        {
            System.IO.StreamReader f = new System.IO.StreamReader(csvPath);
            string line;
            int i = 0;
            while ((line = f.ReadLine()) != null)
            {
                pilot.addLine(line);

                List<float> row = new List<float>();
                string[] values = line.Split(',');
                int j = 0;
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

        public float getCurrentData(String feature)
        {
            return flightdata.getValue(feature, timestep);
        }
        public bool SendCurrentData()
        {
            return pilot.sendCurrentData(Timestep);
        }
        public bool startClient()
        {
            return pilot.startClient();
        }

        public void endClient()
        {
            pilot.endClient();
        }

        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath)
        {
            research.analyzeData(normalFlightPath, newFlightPath, anomalyDetPath);
        }

        public String getCorrelative(String featureName)
        {
            return research.getCorrelative(featureName);
        }

        public List<String> getFeaturesList()
        {
            return research.getFeaturesList();
        }

        public List<int> getAnomaliesList(String featureName)
        {
            return research.getAnomaliesList(featureName);
        }

        public List<DataPoint> getDataPoints(String featureName)
        {
            return research.getDataPoints(Timestep, featureName);
        }

        public List<ScatterPoint> getRecentScatterPoints(String featureName)
        {
            return research.getRecentScatterPoints(Timestep, featureName);
        }

        public List<ScatterPoint> getRecentAnomalousPoints(String featureName)
        {
            return research.getRecentAnomalousPoints(Timestep, featureName);
        }

        public Annotation getFeatureAnnotation(String featureName)
        {
            return research.getFeatureAnnotation(featureName);
        }

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

        public void reset()
        {
            flightdata.reset();
            pilot.reset();
            research.reset();
            Timestep = 0;
        }
    }
}
