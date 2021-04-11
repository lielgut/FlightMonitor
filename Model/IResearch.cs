using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;

namespace ex1.Model
{
    interface IResearch
    {
        public float CorrThreshold { get; set; }
        public void addFeature(string featureName);
        public String getFeature(int i);
        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath);
        public String getCorrelative(String featureName);
        public void addData(int featureNum, float val);
        public List<String> getFeaturesList();
        public List<int> getAnomaliesList(String featureName);
        public float getValue(int timestep, String featureName);
        public List<DataPoint> getDataPoints(int timestep, String featureName);
        public List<ScatterPoint> getRecentScatterPoints(int timestep, String featureName);
        public List<ScatterPoint> getRecentAnomalousPoints(int timestep, String featureName);
        public Annotation getFeatureAnnotation(String featureName);
        public double getMinX(String featureName);
        public double getMaxX(String featureName);
        public double getMinY(String featureName);
        public double getMaxY(String featureName);
        public void reset();
    }
}
