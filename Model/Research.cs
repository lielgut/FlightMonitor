using System;
using System.Collections.Generic;
using System.Reflection;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;
using System.IO;
using System.Linq;

namespace ex1.Model
{
    // class for implementing the IResearch interface
    class Research : IResearch
    {
        // nested class for saving information on each feature
        class ResearchData
        {
            // name of correlated feature
            public String Correlated { get; set; }
            // list of datapoints (x = time, y = value)
            public List<DataPoint> DataPoints { get; set; }
            // list of ScatterPoints for anomalous correlation points
            public List<ScatterPoint> Anomalies { get; set; }
            // list of timesteps with anomalies
            public List<int> AnomaliesTimesteps { get; set; }
            // list of ScatterPoints for correlation points
            public List<ScatterPoint> CorrPoints { get; set; }
            // annotation for feature
            public Annotation PlotAnnotation { get; set; }
            // min/max x,y values
            public float MinX { get; set; }
            public float MinY { get; set; }
            public float MaxX { get; set; }
            public float MaxY { get; set; }
            // constructor
            public ResearchData()
            {
                Correlated = null;
                DataPoints = new List<DataPoint>();
                CorrPoints = new List<ScatterPoint>();
                Anomalies = new List<ScatterPoint>();
                AnomaliesTimesteps = new List<int>();             
                PlotAnnotation = null;
            }
        }

        // the pearson threshold by which features are considred correlative
        private float corrThreshold;
        public float CorrThreshold
        {
            get
            {
                return corrThreshold;
            }
            set
            {
                corrThreshold = value;
            }
        }
        
        // list of features
        private List<String> features;
        // mapping features to their ResearchData
        private Dictionary<String, ResearchData> dataDict;

        // constructor
        public Research()
        {
            features = new List<String>();
            dataDict = new Dictionary<string, ResearchData>();
        }

        // add a new feature to list
        public void addFeature(string featureName)
        {
            features.Add(featureName);
            ResearchData rd = new ResearchData();           
            dataDict[featureName] = rd;
        }

        // get a feature name of given column number
        public string getFeature(int i)
        {
            return features[i];
        }

        // add given value at given column number
        public void addData(int featureNum, float val)
        {           
            List<DataPoint> l = dataDict[features[featureNum]].DataPoints;
            l.Add(new DataPoint(l.Count, val));
        }

        // analyze data from CSV files and detect anomalies by given plugin
        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath)
        {
                        
            // append feature names to files and save in resources folder

            string featuresStr = "";
            int i;
            for (i = 0; i < features.Count - 1; i++)
            {
                featuresStr += features[i] + ",";
            }
            featuresStr += features[i];

            string s;
            StreamReader normalFlightFile = new StreamReader(normalFlightPath);
            String pathReg = @"..\..\..\Resources\regFlightWithFeatures.csv";
            StreamWriter normalFlightFile1 = new StreamWriter(pathReg);            

            normalFlightFile1.WriteLine(featuresStr);
            while ((s = normalFlightFile.ReadLine()) != null)
            {
                normalFlightFile1.WriteLine(s);
            }

            normalFlightFile.Close();
            normalFlightFile1.Close();

            s = "";
            StreamReader newFlightFile = new StreamReader(newFlightPath);
            String pathNew = @"..\..\..\Resources\newFlightWithFeatures.csv";
            StreamWriter newFlightFile1 = new StreamWriter(pathNew);

            newFlightFile1.WriteLine(featuresStr);     
            while ((s = newFlightFile.ReadLine()) != null)
            {
                newFlightFile1.WriteLine(s);
            }

            newFlightFile.Close();
            newFlightFile1.Close();


            // load dll
            Assembly asm = Assembly.LoadFile(anomalyDetPath);
            Type detectorType = asm.GetType("Detector.Detector");

            // create instance of the Detector class from dll
            object detector = Activator.CreateInstance(detectorType);

            // load and analyze data from CSV files
            MethodInfo loadFlightData = detectorType.GetMethod("loadFlightData");
            loadFlightData.Invoke(detector, new object[] { pathReg, pathNew, CorrThreshold });

            // learn correlated features, plot models, anomalies
            MethodInfo getCorrFeature = detectorType.GetMethod("getCorrFeature");
            MethodInfo getAnnotation = detectorType.GetMethod("getAnnotation");
            MethodInfo isFeatureAnomalous = detectorType.GetMethod("isFeatureAnomalous");
            int len = dataDict[features[0]].DataPoints.Count;

            // iterate through feature names
            foreach (String featureName in features)
            {
                ResearchData rd = dataDict[featureName];

                // get correlative feature
                String correlated = getCorrFeature.Invoke(detector, new object[] { featureName }) as String;
                rd.Correlated = correlated;
                
                // if a correlative feature exists
                if (correlated != null)
                {
                    float minX = 0, minY = 0, maxX = 0, maxY = 0;
                    for (i = 0; i < len; i++)
                    {
                        // get x,y values
                        float x = getValue(i, featureName), y = getValue(i, correlated);

                        // check if point is anomalous
                        bool isAnom = (bool) isFeatureAnomalous.Invoke(detector, new object[] { i, featureName });
                        if (isAnom)
                        {
                            // if anomalous add to anomalies, add null to other correlation points
                            rd.CorrPoints.Add(null);
                            rd.Anomalies.Add(new ScatterPoint(x, y, 2));
                            // add timestep to anomalies
                            rd.AnomaliesTimesteps.Add(i);
                        } else
                        {
                            // if not anomalous add to correlation points, add null to anomalous points
                            rd.CorrPoints.Add(new ScatterPoint(x, y, 2));
                            rd.Anomalies.Add(null);
                        }                                                                      

                        // update min/max x,y values
                        if (x < minX)
                            minX = x;
                        else if (x > maxX)
                            maxX = x;
                        if (y < minY)
                            minY = y;
                        else if (y > maxY)
                            maxY = y;                        
                    }

                    // get and set annotation for feature
                    Annotation annot = getAnnotation.Invoke(detector, new object[] { featureName }) as Annotation;                                      
                    rd.PlotAnnotation = annot;

                    // set min/max x,y values
                    rd.MinX = minX;
                    rd.MaxX = maxX;
                    rd.MinY = minY;
                    rd.MaxY = maxY;
                }                               
            }
            
            // delete any memory allocated by plugin
            MethodInfo deleteAH = detectorType.GetMethod("deleteAnomalyHelper");
            deleteAH.Invoke(detector, null);
        }

        // get name of feature correlative to given feature
        public string getCorrelative(string featureName)
        {
            return dataDict[featureName].Correlated;
            // return corrFeatures[featureName];
        }

        // get a list of the features names
        public List<String> getFeaturesList()
        {
            return features;
        }

        // get a list of timesteps with anomalies
        public List<int> getAnomaliesList(String featureName)
        {
            if(featureName == null)
            {
                return null;
            }
            return dataDict[featureName].AnomaliesTimesteps;
        }

        // get a value from specific timestep and feature
        public float getValue(int timestep, String featureName)
        {
            if (featureName == null || timestep == dataDict[featureName].DataPoints.Count)
                return 0;
            return (float)dataDict[featureName].DataPoints[timestep].Y;
        }

        // get a list of datapoints for specific timestep and featureName
        public List<DataPoint> getDataPoints(int timestep, String featureName)
        {
            if(featureName == null)
            {
                return null;
            }
            return new List<DataPoint>(dataDict[featureName].DataPoints.Take(timestep));
        }

        // get a list of points for specific timestep and featureName from recent 30 seconds
        public List<ScatterPoint> getRecentScatterPoints(int timestep, String featureName)
        {
            if (featureName == null)
            {
                return null;
            }
            return new List<ScatterPoint>(dataDict[featureName].CorrPoints
                .Take(timestep).Skip(timestep > 300 ? timestep - 300 : 0).Where(point => point != null));
        }

        // get a list of anomalous points for specific timestep and feature name from recent 30 seconds
        public List<ScatterPoint> getRecentAnomalousPoints(int timestep, String featureName)
        {
            if (featureName == null)
            {
                return null;
            }
            return new List<ScatterPoint>(dataDict[featureName].Anomalies
                .Take(timestep).Skip(timestep > 300 ? timestep - 300 : 0).Where(point => point != null)); ;
        }

        // get an annotation of given feature name
        public Annotation getFeatureAnnotation(String featureName)
        {
            if(featureName == null)
            {
                return null;
            }
            return dataDict[featureName].PlotAnnotation;
        }

        // return minimal/maximal x/y values of given feature's correlation points
        public double getMinX(String featureName)
        {
            return dataDict[featureName].MinX;
        }
        public double getMaxX(String featureName)
        {
            return dataDict[featureName].MaxX;
        }
        public double getMinY(String featureName)
        {
            return dataDict[featureName].MinY;
        }
        public double getMaxY(String featureName)
        {
            return dataDict[featureName].MaxY;
        }

        // clear all loaded data
        public void reset()
        {
            foreach (KeyValuePair<string, ResearchData> entry in dataDict)
            {
                entry.Value.Correlated = null;
                entry.Value.DataPoints.Clear();
                entry.Value.CorrPoints.Clear();
                entry.Value.Anomalies.Clear();
                entry.Value.AnomaliesTimesteps.Clear();
                entry.Value.PlotAnnotation = null;
            }
        }
    }
}
