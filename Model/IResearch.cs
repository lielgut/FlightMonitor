using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace ex1.Model
{
    interface IResearch
    {
        public float getValue(String feature, int timestep);
        public void addValue(String feature, float value);
        public String getCorrelatedFeature(String feature);
        public void learnNormal(string path);
        public bool isAnAnomaly(int timestep, String feature);
        public Line getLinearRegLine(String feature);
        public Circle getMinimalCircle(String feature);
    }
}
