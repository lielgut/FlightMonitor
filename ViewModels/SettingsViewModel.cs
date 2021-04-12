using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class SettingsViewModel : ViewModel
    {
        public SettingsViewModel(IFlightControl model) : base(model) { }

        public PathInfo VM_Paths
        {
            get
            {
                return Model.Paths;
            }
        }

        public int VM_DestPort
        {
            get
            {
                return Model.DestPort;
            }
            set
            {
                Model.DestPort = value;
                PropertyChangedNotify("VM_DestPort");
            }
        }

        public float VM_Threshold
        {
            set
            {
                Model.Threshold = value;
            }
        }

        public bool StartClient()
        {
            return Model.startClient();
        }

        public void EndClient()
        {
            Model.endClient();
        }

        public void Reset()
        {
            Model.reset();
        }

        public void LoadFeatures(String path)
        {
            Model.loadFeatures(path);
        }

        public void LoadData()
        {
            Model.loadData(VM_Paths.NewCSVPath);
        }

        public void AnalyzeData()
        {
            Model.analyzeData(VM_Paths.NormalCSVPath, VM_Paths.NewCSVPath, VM_Paths.DLLPath);
        }

        public MainWindow LoadMainWindow()
        {
            return new MainWindow(Model);
        }
    }
}
