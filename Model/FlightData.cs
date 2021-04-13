using System;
using System.Collections.Generic;

namespace ex1.Model
{
    // class for implementing the IFlightData interface
    class FlightData : IFlightData
    {
        // list of data lists (rows are timesteps, columns are features)
        List<List<float>> data;
        // mapping feature names to their column number
        Dictionary<String, int> features;

        // constructor
        public FlightData()
        {
            this.data = new List<List<float>>();
            this.features = new Dictionary<string, int>();
        }

        // add list of data for last timestep
        public void addData(List<float> l)
        {
            data.Add(l);
        }

        // get value of feature at given timestep
        public float getValue(string featureName, int timestep)
        {
            if (timestep == data.Count || featureName == null || !features.ContainsKey(featureName))
                return 0;
            return data[timestep][features[featureName]];
        }

        // add mapping between feature and column
        public void addFeature(string featureName, int column)
        {
            features.Add(featureName, column);
        }

        // return if feature names contain given name
        public bool containsFeature(string featureName)
        {
            return features.ContainsKey(featureName);
        }

        // clear all loaded data
        public void reset()
        {
            data.Clear();
        }
    }
}
