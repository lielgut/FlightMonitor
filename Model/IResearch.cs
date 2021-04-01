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
        public void addData(string featureName, float val);
        public PlotModel getPlotModel(int timestep, string featureName);

    }
}
