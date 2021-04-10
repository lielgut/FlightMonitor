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
    class Research : IResearch
    {
        class ResearchData
        {
            public String Correlated { get; set; }
            public List<DataPoint> DataPoints { get; set; }
            public List<ScatterPoint> Anomalies { get; set; }
            public List<int> AnomaliesTimesteps { get; set; }
            public List<ScatterPoint> CorrPoints { get; set; }
            public Annotation PlotAnnotation { get; set; }
            public float MinX { get; set; }
            public float MinY { get; set; }
            public float MaxX { get; set; }
            public float MaxY { get; set; }
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

        private List<String> features;
        public Research()
        {
            features = new List<String>();
            dataDict = new Dictionary<string, ResearchData>();
        }
        private Dictionary<String, ResearchData> dataDict;


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
            // dataDict[features[featureNum]].DataVector.Add(val);
            List<DataPoint> l = dataDict[features[featureNum]].DataPoints;
            l.Add(new DataPoint(l.Count, val));
        }

        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath)
        {
                        
            // append feature names to files

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


            // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // 

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
            foreach (String featureName in features)
            {
                ResearchData rd = dataDict[featureName];
                String correlated = getCorrFeature.Invoke(detector, new object[] { featureName }) as String;
                rd.Correlated = correlated;
                

                if (correlated != null)
                {
                    float minX = 0, minY = 0, maxX = 0, maxY = 0;
                    for (i = 0; i < len; i++)
                    {
                        float x = getValue(i, featureName), y = getValue(i, correlated);

                        bool isAnom = (bool) isFeatureAnomalous.Invoke(detector, new object[] { i, featureName });
                        if (isAnom)
                        {
                            rd.CorrPoints.Add(null);
                            rd.Anomalies.Add(new ScatterPoint(x, y, 2));
                            rd.AnomaliesTimesteps.Add(i);
                        } else
                        {
                            rd.CorrPoints.Add(new ScatterPoint(x, y, 2));
                            rd.Anomalies.Add(null);
                        }                                                                      

                        if (x < minX)
                            minX = x;
                        else if (x > maxX)
                            maxX = x;
                        if (y < minY)
                            minY = y;
                        else if (y > maxY)
                            maxY = y;                        
                    }

                    Annotation annot = getAnnotation.Invoke(detector, new object[] { featureName }) as Annotation;                                      
                    rd.PlotAnnotation = annot;
                    rd.MinX = minX;
                    rd.MaxX = maxX;
                    rd.MinY = minY;
                    rd.MaxY = maxY;
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

        public List<int> getAnomaliesList(String featureName)
        {
            if(featureName == null)
            {
                return null;
            }
            return dataDict[featureName].AnomaliesTimesteps;
        }

        /*public bool isAnomalous(int timestep, string featureName)
        {
            return dataDict[featureName].Anomalies[timestep];
        }*/

        public float getValue(int timestep, String featureName)
        {
            if (featureName == null || timestep == dataDict[featureName].DataPoints.Count)
                return 0;
            return (float)dataDict[featureName].DataPoints[timestep].Y;
        }

        public List<DataPoint> getDataPoints(int timestep, String featureName)
        {
            if(featureName == null)
            {
                return null;
            }
            return new List<DataPoint>(dataDict[featureName].DataPoints.Take(timestep));
        }

        public List<ScatterPoint> getRecentScatterPoints(int timestep, String featureName)
        {
            if (featureName == null)
            {
                return null;
            }
            return new List<ScatterPoint>(dataDict[featureName].CorrPoints
                .Take(timestep).Skip(timestep > 300 ? timestep - 300 : 0).Where(point => point != null));
        }

        public List<ScatterPoint> getRecentAnomalousPoints(int timestep, String featureName)
        {
            if (featureName == null)
            {
                return null;
            }
            return new List<ScatterPoint>(dataDict[featureName].Anomalies
                .Take(timestep).Skip(timestep > 300 ? timestep - 300 : 0).Where(point => point != null)); ;
        }

        public Annotation getFeatureAnnotation(String featureName)
        {
            if(featureName == null)
            {
                return null;
            }
            return dataDict[featureName].PlotAnnotation;
        }

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
