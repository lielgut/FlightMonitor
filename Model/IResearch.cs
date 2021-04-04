using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Wpf;

namespace ex1.Model
{
    interface IResearch
    {
        public void addFeature(string featureName);
        public String getFeature(int i);
        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath);
        public String getCorrelative(String featureName);
        public void addData(int featureNum, float val);
        public List<String> getFeaturesList();
        public bool isAnomalous(int timestep, string featureName);
        public float getValue(int timestep, String featureName);
        public List<DataPoint> getDataPoints(int timestep, String featureName);
        public Annotation getFeatureAnnotation(String featureName);
    }
}
