using System.Collections.Generic;

namespace ex1.Model
{
    interface IFlightData
    {
        // add list of data for last timestep
        public void addData(List<float> l);

        // get value of feature at given timestep
        public float getValue(string featureName, int timestep);

        // add mapping between feature and column
        public void addFeature(string featureName, int column);

        // return if feature names contain given name
        public bool containsFeature(string featureName);

        // clear all loaded data
        public void reset();
    }
}
