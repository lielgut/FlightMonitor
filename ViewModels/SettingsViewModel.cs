using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class SettingsViewModel : ViewModel
    {
        public SettingsViewModel(IFlightControl fc) : base(fc) { }

        public PathInfo VM_Paths
        {
            get
            {
                return fc.Paths;
            }
        }

        public int VM_DestPort
        {
            set
            {
                fc.DestPort = value;
            }
        }

        public float VM_Threshold
        {
            set
            {
                fc.Threshold = value;
            }
        }

        public bool StartClient()
        {
            return fc.startClient();
        }

        public void EndClient()
        {
            fc.endClient();
        }

        public void Reset()
        {
            fc.reset();
        }

        public void LoadFeatures(String path)
        {
            fc.loadFeatures(path);
        }

        public void LoadData()
        {
            fc.loadData(VM_Paths.NewCSVPath);
        }

        public void AnalyzeData()
        {
            fc.analyzeData(VM_Paths.NormalCSVPath, VM_Paths.NewCSVPath, VM_Paths.DLLPath);
        }

        public MainWindow LoadMainWindow()
        {
            return new MainWindow(fc);
        }
    }
}
