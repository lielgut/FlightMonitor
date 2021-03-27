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
            List<float> l = new List<float>();
            string[] values = s.Split(',');            
            foreach (string val in values)
            {
                l.Add(float.Parse(val));
            }
            data.Add(l);
        }

        // TODO implement
        public float getValue(string featureName, int timestep)
        {
            return data[features[featureName]][timestep];
        }
        // TODO implement
        public void addFeature(string featureName, int column)
        {
            features.Add(featureName, column);
        }

        public bool containsFeature(string featureName)
        {
            return features.ContainsKey(featureName);
        }
    }
}
