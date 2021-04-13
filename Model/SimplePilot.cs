using System;
using System.Collections.Generic;

namespace ex1.Model
{
    // class for implementing the Pilot abstract class
    class SimplePilot : Pilot
    {
        // list of strings to be sent to server
        private List<String> dataLines;

        // constructor
        public SimplePilot()
        {
            this.cl = new SimpleClient();
            this.dataLines = new List<string>();
        }

        // add a string to the data that will be sent to server
        public override void addLine(string s)
        {
            dataLines.Add(s);
        }

        // get string of given timestep and send it via client
        public override bool sendCurrentData(int timestep)
        {
            return cl.send(dataLines[timestep]);
        }

        // clear loaded data
        public override void reset()
        {
            dataLines.Clear();
        }
    }
}
