using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using OxyPlot;

namespace ex1.Model
{
    interface IFlightControl : INotifyPropertyChanged
    {
        float Speed { get; set; }
        int Timestep { get; set; } 
        int NumLines { get; set; }
        bool IsReverse { get; set; }
        bool Stop { get; set; }
        public void start();
        public void stop();
        public void loadFeatures(string xmlPath);
        public void loadData(string csvPath);
        public void changePort(int destPort);
        public float getCurrentData(String feature);
        public void SendCurrentData();
        public bool startClient();
        public void endClient();
        public List<String> getFeaturesList();
        public PlotModel getCurrentPlot(String feature);
    }
}
