using System;
using System.Collections.Generic;
using System.Text;

namespace ex1.Model
{
    interface IFlightData
    {
        // parse and add data
        public void addData(List<float> l);
        // get value of feature at given timestep
        public float getValue(string featureName, int timestep);
        // add feature-column mapping
        public void addFeature(string featureName, int column);
        // returns if feature names contain given name
        public bool containsFeature(string featureName);
    }
}
