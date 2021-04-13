using BespokeFusion;
using ex1.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ex1.Views
{
    // view for applying new settings
    public partial class SettingsView : UserControl
    {
        // the settings view model is used for getting/setting configurations
        private SettingsViewModel _settingsVM;
        internal SettingsViewModel SettingsVM
        {
            get
            {
                return _settingsVM;
            }
            set
            {
                _settingsVM = value;
            }
        }

        // background music player
        private MediaPlayer mp;
        private bool isMpPlaying;

        // constructor for the settings view
        public SettingsView()
        {
            InitializeComponent();
            mp = new MediaPlayer();
            mp.Open(new Uri(@"..\..\..\Resources\surprise.mp3", UriKind.Relative));
            isMpPlaying = false;
        }

        // browse for a new CSV file of a normal flight
        private void BrowseNormal_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "CSV file (*.csv)|*.csv";
            browser.ShowDialog();
            normalFlightPath.Text = browser.FileName;
        }

        // browse for a new CSV file of a new flight
        private void BrowseNew_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "CSV file (*.csv)|*.csv";
            browser.ShowDialog();
            newFlightPath.Text = browser.FileName;
        }

        // browse for a new plugin DLL file
        private void BrowseAnomaly_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "Plugin file (*.dll)|*.dll";
            browser.ShowDialog();
            anomalyDetPath.Text = browser.FileName;
        }

        // event for when the "Launch FlightGear" button is clicked
        private void LaunchFG_Click(object sender, RoutedEventArgs e)
        {
            // verify FG is installed
            if (!File.Exists(_settingsVM.VM_Paths.FGPath + @"\bin\fgfs.exe"))
            {
                MaterialMessageBox.ShowError("Couldn't locate fgfs.exe in FlightGear installation folder.");
                return;
            }

            // verify port number
            int pn;
            try
            {
                pn = Int32.Parse(portnum.Text);
                if (pn < 1024 || pn > 65535)
                {
                    MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535.");
                    return;
                }                
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid input for port number.");
                return;
            }

            // verify FlightGear isn't already running
            Process[] pname = Process.GetProcessesByName("fgfs");
            if (pname.Length > 0)
            {               
                CustomMaterialMessageBox msg = new CustomMaterialMessageBox
                {
                    TxtTitle = { Text = "Warning", Foreground = Brushes.White },
                    TxtMessage = { Text = "FlightGear is already running, please close it and try again.", Foreground = Brushes.Black },
                    BtnOk = { Content = "Kill FG" },
                    BtnCancel = { Content = "Cancel" },
                    MainContentControl = { Background = Brushes.WhiteSmoke },
                    TitleBackgroundPanel = { Background = Brushes.MediumPurple },
                    BorderBrush = Brushes.BlueViolet
                };

                msg.Show();
                MessageBoxResult results = msg.Result;
                // allow user to kill FG process if running
                if (results == MessageBoxResult.OK)
                {
                    foreach (var process in pname)
                        process.Kill();
                }
                return;
            }
            // try to start FlightGear
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(_settingsVM.VM_Paths.FGPath + @"\bin\fgfs.exe", "--generic=socket,in,10,127.0.0.1," + pn + ",tcp,playback_small --fdm=null --timeofday=morning");
                psi.WorkingDirectory = _settingsVM.VM_Paths.FGPath + @"\data";
                Process.Start(psi);

            }
            catch (Exception)
            {
                MaterialMessageBox.ShowError("An error has occured, please make sure that FlightGear is installed properly.");
            } 
        }

        // event for when the apply CSV button is clicked
        private void ApplyCSV_Click(object sender, RoutedEventArgs e)
        {
            // verify paths for both CSV files
            if (normalFlightPath.Text == "" || !File.Exists(normalFlightPath.Text))
            {
                MaterialMessageBox.ShowError("Please select normal flight CSV file.");
                return;
            }
            if (newFlightPath.Text == "" || !File.Exists(newFlightPath.Text))
            {
                MaterialMessageBox.ShowError("Please select new flight CSV file.");
                return;
            }
            // set the paths
            _settingsVM.VM_Paths.NormalCSVPath = normalFlightPath.Text;
            _settingsVM.VM_Paths.NewCSVPath = newFlightPath.Text;

            // reset any previous data loaded from other files
            reset();

            // notify user CSV files were loaded
            MaterialMessageBox.Show("new files loaded succesfully");

            // reset textboxes
            normalFlightPath.Text = "";
            newFlightPath.Text = "";
        }

        // event for when the apply DLL button is clicked
        private void ApplyDLL_Click(object sender, RoutedEventArgs e)
        {
            // verify plugin path
            if (anomalyDetPath.Text == "" || !File.Exists(anomalyDetPath.Text))
            {
                MaterialMessageBox.ShowError("Please select anomaly detection dll.");
                return;
            }
            // verify a threshold was entered
            if (thresholdText.Text == "")
            {
                MaterialMessageBox.ShowError("Please choose a correlation threshold.");
                return;
            }
            float threshold;
            // verify that threshold value is valid
            try
            {
                threshold = float.Parse(thresholdText.Text);
                if (threshold <= 0 || threshold >= 1)
                {
                    MaterialMessageBox.ShowError("Threshold value must be larger than 0 and smaller than 1.");
                    return;
                }
                // set new threshold
                _settingsVM.VM_Threshold = threshold;
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid input for threshold value. Please enter a number between 0-1.");
                return;
            }

            // set new plugin path
            _settingsVM.VM_Paths.DLLPath = anomalyDetPath.Text;           
            // reset data in order and analyze with new plugin
            reset();

            // notify user plugin was loaded
            MaterialMessageBox.Show("plugin loaded succesfully");

            // reset textboxes
            anomalyDetPath.Text = "";
            thresholdText.Text = "";
        }

        // event for when the apply port button is clicked
        private void ApplyPort_Click(object sender, RoutedEventArgs e)
        {
            // verify port number
            int pn;
            try
            {
                pn = Int32.Parse(portnum.Text);
                if (pn < 1024 || pn > 65535)
                {
                    MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                    return;
                }                
            }

            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                return;
            }

            // save old port
            int oldPort = _settingsVM.VM_DestPort;

            // end previous conection with server
            _settingsVM.EndClient();      
            
            // set new destination port
            _settingsVM.VM_DestPort = pn;

            // try to start client and connect to new port
            if(_settingsVM.StartClient())
                MaterialMessageBox.Show("Connected to new port");
            else
            {
                // if connection failed set to previous port
                _settingsVM.VM_DestPort = oldPort;
                MaterialMessageBox.ShowError("Failed to connect to new port. Please try again.");                
            }         
            
            // reset textbox
            portnum.Text = "";
        }

        // remove data from previous files, load and analyze the new data
        private void reset()
        {
            _settingsVM.Reset();
            _settingsVM.LoadData();
            _settingsVM.AnalyzeData();
        }

        // event for clicking the easter egg (try to find it!)
        private void EasterEgg_Click(object sender, RoutedEventArgs e)
        {
            if(!isMpPlaying)
            {                
                mp.Play();
                isMpPlaying = true;
            }
            else
            {
                mp.Pause();
                isMpPlaying = false;
            }
        }        
    }
}
