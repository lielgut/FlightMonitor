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
            }
        }        

        private volatile bool stop;
        private int numLines;
        private Pilot pilot;
        private IFlightData flightdata;
        private IResearch research;
        Thread thread;
        public event PropertyChangedEventHandler PropertyChanged;

        public FlightControl()
        {
            this.speed = 1;
            this.timestep = 0;
            this.stop = false;
            this.pilot = new SimplePilot();
            this.flightdata = new FlightData();
            this.research = new Research();            
        }


        public void start()
        {
            this.thread = new Thread(delegate ()
           {
               while (!stop)
               {
                   if ((speed == 0) || (Timestep == 0 && speed < 0) || (Timestep >= numLines && speed > 0))
                   {
                       stop = true;
                       break;
                   }                   
                   pilot.sendCurrentData(Timestep);
                   Timestep += (speed > 0 ? 1 : -1);
                   Thread.Sleep((int)(Math.Abs(speed) * 1f));
               }
           });
            thread.Start();
        }

        void IFlightControl.stop()
        {
            this.stop = true;
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
            while((line = f.ReadLine()) != null)
            {
                pilot.addLine(line);
                flightdata.addData(line);
                // add research data
                i++;
            }
            numLines = i;            
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
