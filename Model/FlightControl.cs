using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Xml;

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

        private bool isReverse;
        public bool IsReverse
        {
            get
            {
                return IsReverse;
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
            set { stop = value; }
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
        }


        public void start()
        {
            stop = false;
            this.thread = new Thread(delegate ()
            {
                while (!stop)
                {
                    bool IsReverse = Speed < 0;
                    if ((Timestep == 0 && IsReverse) || (Timestep >= numLines && !IsReverse))
                    {
                        stop = true;
                        break;
                    }
                    pilot.sendCurrentData(Timestep);
                    Timestep += (IsReverse ? -1 : 1);
                    Thread.Sleep((int)(100f / Math.Abs(Speed)));
                }
            });
            thread.Start();
        }

        void IFlightControl.stop()
        {
            stop = true;
            thread.Join();
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
                flightdata.addData(line);
                // add research data
                i++;
            }
            NumLines = i;
            f.Close();
        }

        public void changePort(int destPort)
        {
            this.pilot.changePort(destPort);
        }

        public float getCurrentData(String feature)
        {
            return flightdata.getValue(feature, timestep);
        }

        public bool startClient()
        {
            return pilot.startClient();
        }

        public void endClient()
        {
            pilot.endClient();
        }
    }
}
