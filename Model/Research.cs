using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace ex1.Model
{
    class Circle
    {
        private float cx;
        private float cy;
        private float radius;
        public Circle(float cx, float cy, float radius)
        {
            this.cx = cx;
            this.cy = cy;
            this.radius = radius;
        }
    }
    class Research : IResearch
    {
        private Dictionary<String, List<float>> featuresData;
        private Dictionary<String, String> correlatedFeatures;
        private Dictionary<Tuple<int, String>, bool> anomalies;
        private Dictionary<String, Line> linearRegLines;
        private Dictionary<String, Circle> minimalCircles;

        public Research()
        {
            this.featuresData = new Dictionary<String, List<float>>();
            this.correlatedFeatures = new Dictionary<string, string>();
            this.anomalies = new Dictionary<Tuple<int, string>, bool>();
            this.linearRegLines = new Dictionary<string, Line>();
            this.minimalCircles = new Dictionary<string, Circle>();
        }

        public void addValue(string feature, float value)
        {
            featuresData[feature].Add(value);
        }

        public string getCorrelatedFeature(string feature)
        {
            return correlatedFeatures[feature];
        }

        public Line getLinearRegLine(string feature)
        {
            return linearRegLines[feature];
        }

        public Circle getMinimalCircle(string feature)
        {
            return minimalCircles[feature];
        }

        public float getValue(string feature, int timestep)
        {
            return featuresData[feature][timestep];
        }

        public bool isAnAnomaly(int timestep, string feature)
        {
            Tuple<int, String> t = new Tuple<int, string>(timestep, feature);
            return anomalies[t];
        }
        // TO DO implement
        public void learnNormal(string path)
        {
            throw new NotImplementedException();
        }
    }
}
