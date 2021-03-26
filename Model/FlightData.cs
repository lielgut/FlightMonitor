using System;
using System.Collections.Generic;
using System.Text;

namespace ex1.Model
{
    class FlightData : IFlightData
    {
        List<List<float>> data;
        Dictionary<String, int> features;

        public FlightData()
        {
            this.data = new List<List<float>>();
            this.features = new Dictionary<string, int>();
        }

        // TODO implement
        public void addData(string s)
        {
            throw new NotImplementedException();
        }

        // TODO implement
        public float getValue(string featureName, int timestep)
        {
            throw new NotImplementedException();
        }
    }
}
