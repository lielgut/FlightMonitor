﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using System.Windows.Media;

namespace ex1.Model
{
    class Research : IResearch
    {
        class ResearchData
        {
            public String Correlated { get; set; }
            public List<float> DataVector { get; set; }
            public List<bool> Anomalies { get; set; }
            public PlotModel Plot { get; set; }
            public PlotModel Series { get; set; }
            public ResearchData()
            {
                Correlated = null;
                DataVector = new List<float>();
                Anomalies = new List<bool>();
                Series = new PlotModel();
                Series.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 0, Position = OxyPlot.Axes.AxisPosition.Bottom });
                Series.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 0, Position = OxyPlot.Axes.AxisPosition.Left });
                Series.Series.Add(new LineSeries());
                Plot = null;
            }
        }

        private List<String> features;
        private Dictionary<String, ResearchData> dataDict;

        public Research()
        {
            features = new List<String>();
            dataDict = new Dictionary<string, ResearchData>();
        }

        public void addFeature(string featureName)
        {
            features.Add(featureName);
            ResearchData rd = new ResearchData();           
            dataDict[featureName] = rd;
        }

        public string getFeature(int i)
        {
            return features[i];
        }

        public void addData(int featureNum, float val)
        {
            dataDict[features[featureNum]].DataVector.Add(val);
        }

        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath)
        {
            // load dll
            Assembly asm = Assembly.LoadFile(anomalyDetPath);
            Type detectorType = asm.GetType("Detector.Detector");

            // create instance of the Detector class from dll
            object detector = Activator.CreateInstance(detectorType);

            // load and analyze data from CSV files
            MethodInfo loadFlightData = detectorType.GetMethod("loadFlightData");
            String temp = @"‪F:\Desktop\reg_flight0.csv";
            loadFlightData.Invoke(detector, new object[] { temp, temp });

            // learn correlated features, plot models, anomalies
            MethodInfo getCorrFeature = detectorType.GetMethod("getCorrFeature");
            MethodInfo getPlotModel = detectorType.GetMethod("getPlotModel");
            MethodInfo isFeatureAnomalous = detectorType.GetMethod("isFeatureAnomalous");
            int len = dataDict[features[0]].DataVector.Count;
            foreach (String featureName in features)
            {                
                String correlated = getCorrFeature.Invoke(detector, new object[] { featureName }) as String;
                // corrFeatures[featureName] = correlated as String;
                dataDict[featureName].Correlated = correlated;

                List<float> featureData = dataDict[featureName].DataVector;
                

                if (correlated != null)
                {
                    List<float> corrFeatureData = dataDict[correlated].DataVector;

                    float minX = 0, minY = 0, maxX = 10, maxY = 10;
                    for (int i = 0; i < len; i++)
                    {
                        object isAnom = isFeatureAnomalous.Invoke(detector, new object[] { i, featureName });
                        dataDict[featureName].Anomalies.Add((bool)isAnom);

                        float x = featureData[i];
                        if (x < minX) { minX = x; }
                        else if (x > maxX) { maxX = x; }

                        float y = corrFeatureData[i];
                        if (y < minY) { minY = y; }
                        else if (y > maxY) { maxY = y; }
                    }

                    PlotModel plot = getPlotModel.Invoke(detector, new object[] { featureName }) as PlotModel;
                    plot.Axes.Add(new LinearAxis { Minimum = minX, Maximum = maxX, Position = OxyPlot.Axes.AxisPosition.Bottom });
                    plot.Axes.Add(new LinearAxis { Minimum = minY, Maximum = maxY, Position = OxyPlot.Axes.AxisPosition.Left });
                    dataDict[featureName].Plot = plot;
                }
                else
                {
                    dataDict[featureName].Anomalies = null;
                }                                
            }
            
            MethodInfo deleteAH = detectorType.GetMethod("deleteAnomalyHelper");
            deleteAH.Invoke(detector, null);
        }

        public string getCorrelative(string featureName)
        {
            return dataDict[featureName].Correlated;
            // return corrFeatures[featureName];
        }

        public List<String> getFeaturesList()
        {
            return features;
        }

        public PlotModel getPlotModel(string featureName)
        {
            if (featureName == null)
            {
                return null;
            }
            PlotModel pm = dataDict[featureName].Plot;
            if (pm == null)
            {
                return null;
            }
            return pm;
        }      
        
        public bool isAnomalous(int timestep, string featureName)
        {
            return dataDict[featureName].Anomalies[timestep];
        }

        public float getValue(int timestep, String featureName)
        {
            return dataDict[featureName].DataVector[timestep];
        }

        public List<DataPoint> getDataPoints(int timestep, String featureName)
        {
            if(featureName == null)
            {
                return null;
            }
            List<DataPoint> l = new List<DataPoint>();
            List<float> values = dataDict[featureName].DataVector; 
            for(int i=0; i < timestep; i++)
            {
                l.Add(new DataPoint(i, values[i]));
            }
            return l;
        }
    }
}
