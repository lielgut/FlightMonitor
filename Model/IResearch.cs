using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using OxyPlot;

namespace ex1.Model
{
    interface IResearch
    {
        public void addFeature(string featureName);
        public string getFeature(int i);
        public void addData(int featureNum, float val);
        public List<String> getFeaturesList();
        public PlotModel getPlotModel(int timestep, string featureName);

    }
}
