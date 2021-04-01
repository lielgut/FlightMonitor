using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using OxyPlot;

namespace ex1.Model
{
    class Research : IResearch
    {
        private List<Dictionary<String, PlotModel>> plots;
        private List<String> features;
        private Dictionary<String, List<float>> data;

        public Research() { }

        public void addFeature(string featureName)
        {
            features.Add(featureName);
            data[featureName] = new List<float>();
        }

        public string getFeature(int i)
        {
            return features[i];
        }

        public void addData(string featureName, float val)
        {
            data[featureName].Add(val);
        }

        public PlotModel getPlotModel(int timestep, string featureName)
        {
            return plots[timestep][featureName];
        }
    }
}
