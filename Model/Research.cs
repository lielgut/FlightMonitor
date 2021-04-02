using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using OxyPlot;

namespace ex1.Model
{
    class Research : IResearch
    {       
        private List<String> features;
        private Dictionary<String, List<float>> data;
        private List<Dictionary<String, PlotModel>> plots;

        public Research()
        {
            features = new List<String>();
            data = new Dictionary<string, List<float>>();
            plots = new List<Dictionary<string, PlotModel>>();
        }

        public void addFeature(string featureName)
        {
            features.Add(featureName);
            data[featureName] = new List<float>();
        }

        public string getFeature(int i)
        {
            return features[i];
        }

        public void addData(int featureNum, float val)
        {
            data[features[featureNum]].Add(val);
        }

        public List<String> getFeaturesList()
        {
            return features;
        }

        public PlotModel getPlotModel(int timestep, string featureName)
        {
            return plots[timestep][featureName];
        }
    }
}
