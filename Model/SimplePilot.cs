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
            this.dataLines = new List<string>();
        }

        // TODO implement
        public override void addLine(string s)
        {            
            throw new NotImplementedException();
        }

        public override int getNumOfLines()
        {
            throw new NotImplementedException();
        }

        // TODO implement
        public override void sendCurrentData(int timestep)
        {
            throw new NotImplementedException();
        }
    }
}
