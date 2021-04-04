using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OxyPlot;
using OxyPlot.Wpf;
using System.Windows.Media;
using System.IO;

namespace ex1.Model
{
    class Research : IResearch
    {
        class ResearchData
        {
            public String Correlated { get; set; }
            public List<float> DataVector { get; set; }
            public List<bool> Anomalies { get; set; }
            public Annotation PlotAnnotation { get; set; }
            public ResearchData()
            {
                Correlated = null;
                DataVector = new List<float>();
                Anomalies = new List<bool>();               
                PlotAnnotation = null;
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
            dataDict[features[featureNum]].DataVector.Add(val);
        }

        public void analyzeData(String normalFlightPath, String newFlightPath, String anomalyDetPath)
        {
            // load dll
            Assembly asm = Assembly.LoadFile(anomalyDetPath);
            Type detectorType = asm.GetType("Detector.Detector");

            // create instance of the Detector class from dll
            object detector = Activator.CreateInstance(detectorType);


            // // // // // // // // // // // // // // // // // // // // // // // // // // // // 
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




            // load and analyze data from CSV files
            MethodInfo loadFlightData = detectorType.GetMethod("loadFlightData");
            loadFlightData.Invoke(detector, new object[] { pathReg, pathNew });

            // learn correlated features, plot models, anomalies
            MethodInfo getCorrFeature = detectorType.GetMethod("getCorrFeature");
            MethodInfo getAnnotation = detectorType.GetMethod("getAnnotation");
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
                    for (i = 0; i < len; i++)
                    {
                        object isAnom = isFeatureAnomalous.Invoke(detector, new object[] { i, featureName });
                        dataDict[featureName].Anomalies.Add((bool)isAnom);
                    }

                    Annotation annot = getAnnotation.Invoke(detector, new object[] { featureName }) as Annotation;                                      
                    dataDict[featureName].PlotAnnotation = annot;
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

        public Annotation getFeatureAnnotation(String featureName)
        {
            return dataDict[featureName].PlotAnnotation;
        }
    }
}
