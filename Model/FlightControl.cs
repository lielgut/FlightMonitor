using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

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
               pilot.startClient();
               while (!stop)
               {
                   if ((speed == 0) || (Timestep == 0 && speed < 0) || (Timestep >= pilot.getNumOfLines() && speed > 0))
                   {
                       stop = true;
                       break;
                   }
                   Timestep += (speed > 0 ? 1 : -1);
                   pilot.sendCurrentData(timestep);
                   Thread.Sleep((int)(Math.Abs(speed) * 100f));
               }
               pilot.endClient();
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public void loadFeatures(string xmlPath)
        {
            throw new NotImplementedException();
        }

        public void loadData(string csvPath)
        {
            throw new NotImplementedException();
        }

        public void changePort(int destPort)
        {
            this.pilot.changePort(destPort);
        }

        public float getCurrentData(String feature)
        {
            throw new Exception();
        }
    }
}
