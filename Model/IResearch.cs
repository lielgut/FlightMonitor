using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;

namespace ex1.Model
{
    // interface for correlations and anomaly detection
    interface IResearch
    {
        // the pearson threshold by which features are considred correlative
        public float CorrThreshold { get; set; }

        // add a new feature to list
        public void addFeature(string featureName);

        // get a feature name of given column number
        public String getFeature(int i);

        // analyze data from CSV files and detect anomalies by given plugin
        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath);

        // get name of feature correlative to given feature
        public String getCorrelative(String featureName);

        // add given value at given column number
        public void addData(int featureNum, float val);

        // get a list of the features names
        public List<String> getFeaturesList();

        // get a list of timesteps with anomalies
        public List<int> getAnomaliesList(String featureName);

        // get a value from specific timestep and feature
        public float getValue(int timestep, String featureName);

        // get a list of datapoints for specific timestep and featureName
        public List<DataPoint> getDataPoints(int timestep, String featureName);

        // get a list of points for specific timestep and featureName from recent 30 seconds
        public List<ScatterPoint> getRecentScatterPoints(int timestep, String featureName);

        // get a list of anomalous points for specific timestep and feature name from recent 30 seconds
        public List<ScatterPoint> getRecentAnomalousPoints(int timestep, String featureName);

        // get an annotation of given feature name
        public Annotation getFeatureAnnotation(String featureName);

        // return minimal/maximal x/y values of given feature's correlation points
        public double getMinX(String featureName);
        public double getMaxX(String featureName);
        public double getMinY(String featureName);
        public double getMaxY(String featureName);

        // clear all loaded data
        public void reset();
    }
}
