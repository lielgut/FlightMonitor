using ex1.Model;

namespace ex1.ViewModels
{
    // the SettingsViewModel is responsible for the presentation logic of the settings view
    class SettingsViewModel : ViewModel
    {
        // view model constructor (contains the model)
        public SettingsViewModel(IFlightControl model) : base(model) { }

        // current saved file paths
        public PathInfo VM_Paths
        {
            get
            {
                return Model.Paths;
            }
        }
        
        // FlightGear destination port
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

        // correlation threshold for anomaly detection
        public float VM_Threshold
        {
            set
            {
                Model.Threshold = value;
            }
        }

        // start client and connect to server (returns false if failed)
        public bool StartClient()
        {
            return Model.startClient();
        }

        // end connection with server
        public void EndClient()
        {
            Model.endClient();
        }

        // remove data from previous files
        public void Reset()
        {
            Model.reset();
        }

        // load feature names from XML file
        public void LoadFeatures()
        {
            Model.loadFeatures(VM_Paths.XMLPath);
        }

        // load data from CSV files
        public void LoadData()
        {
            Model.loadData(VM_Paths.NewCSVPath);
        }
        
        // analyze data and detect anomalies with loaded plugin
        public void AnalyzeData()
        {
            Model.analyzeData(VM_Paths.NormalCSVPath, VM_Paths.NewCSVPath, VM_Paths.DLLPath);
        }

        // create the main window
        public MainWindow LoadMainWindow()
        {
            return new MainWindow(Model);
        }
    }
}
