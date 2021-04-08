using System;
using System.Collections.Generic;
using System.Text;

namespace ex1.Model
{
    abstract class Pilot
    {
        protected IClient cl;
        public bool startClient()
        {
            return cl.connect();
        }
        public void endClient()
        {
            cl.close();
        }
        public void changePort(int newPort)
        {
            cl.DestPort = newPort;
        }
        public int getCurrentPort()
        {
            return cl.DestPort;
        }
        // add string to saved data at last timestep
        public abstract void addLine(string s);
        // find string of given timestep and send it via client
        public abstract bool sendCurrentData(int timestep);
        // reset all data collected
        public abstract void reset();
    }
}
