using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ex1.Model
{
    interface IFlightControl : INotifyPropertyChanged
    {
        float Speed { get; set; }
        int Timestep { get; set; }        
        public void start();
        public void stop();
        public void loadFeatures(string xmlPath);
        public void loadData(string csvPath);
        public void changePort(int destPort);
        public float getCurrentData(String feature);
        public bool startClient();
        public void endClient();
    }
}
