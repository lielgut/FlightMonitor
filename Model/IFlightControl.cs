using System;
using System.Collections.Generic;
using System.ComponentModel;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;

namespace ex1.Model
{
    // the model's facade interface, allowing the view models to interact with the model
    interface IFlightControl : INotifyPropertyChanged
    {

        // properties

        // playback speed
        float Speed { get; set; }

        // current timestep
        int Timestep { get; set; }

        // number of lines in flight file
        int NumLines { get; set; }

        // is flight played in reverse
        bool IsReverse { get; set; }

        // is the flight currently playing
        bool Stop { get; set; }

        // destination port of server
        int DestPort { get; set; }

        // threshold for correlation
        float Threshold { get; set; }

        // paths of files
        PathInfo Paths { get; }
       
        // start playing the flight and updating current timestep
        public void start();

        // stop playing the flight
        public void stop();

        // load features names
        public void loadFeatures(string xmlPath);

        // load data from CSV files
        public void loadData(string csvPath);

        // return value of feature in current timestep
        public float getCurrentData(String feature);

        // send data of current timestep to FlightGear
        public bool SendCurrentData();

        // start the client and connect to server
        public bool startClient();

        // end the client connection with server
        public void endClient();

        // analyze data from CSV files and detect anomalies
        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath);

        // reset all data loaded from files
        public void reset();

        // get name of correlative feature
        public String getCorrelative(String featureName);

        // return the features list
        public List<String> getFeaturesList();

        // return list of timesteps with anomalies
        public List<int> getAnomaliesList(String featureName);

        // return list of points for given feature
        public List<DataPoint> getDataPoints(String featureName);

        // return list of correlation points for given feature from recent 30 seconds
        public List<ScatterPoint> getRecentScatterPoints(String featureName);

        // return list of correlation points for given feature which are anomalous from recent 30 seconds
        public List<ScatterPoint> getRecentAnomalousPoints(String featureName);

        // get the annotation of given feature
        public Annotation getFeatureAnnotation(String featureName);

        // return minimal/maximal x/y values of given feature's correlation points
        public double getMinX(String featureName);
        public double getMaxX(String featureName);
        public double getMinY(String featureName);
        public double getMaxY(String featureName);
    }
}
