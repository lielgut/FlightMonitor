using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ex1.Model
{
    class SimplePilot : Pilot
    {
        private List<String> dataLines;

        public SimplePilot()
        {
            this.cl = new SimpleClient();
            this.dataLines = new List<string>();
        }

        // TODO implement
        public override void addLine(string s)
        {
            dataLines.Add(s);
        }

        // TODO implement
        public override void sendCurrentData(int timestep)
        {
            cl.send(dataLines[timestep]);
        }

        public override void reset()
        {
            dataLines.Clear();
        }
    }
}
