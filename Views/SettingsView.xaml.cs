using BespokeFusion;
using ex1.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace ex1.Views
{
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel settingsVM;
        internal SettingsViewModel SettingsVM
        {
            get
            {
                return settingsVM;
            }
            set
            {
                settingsVM = value;
            }
        }

        private MediaPlayer mp;
        private bool isMpPlaying;

        public SettingsView()
        {
            InitializeComponent();
            mp = new MediaPlayer();
            mp.Open(new Uri(@"..\..\..\Resources\surprise.mp3", UriKind.Relative));
            isMpPlaying = false;
        }


        private void BrowseNormal_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "CSV file (*.csv)|*.csv";
            browser.ShowDialog();
            normalFlightPath.Text = browser.FileName;
        }

        private void BrowseNew_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "CSV file (*.csv)|*.csv";
            browser.ShowDialog();
            newFlightPath.Text = browser.FileName;
        }

        private void BrowseAnomaly_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "Plugin file (*.dll)|*.dll";
            browser.ShowDialog();
            anomalyDetPath.Text = browser.FileName;
        }

        private void LaunchFG_Click(object sender, RoutedEventArgs e)
        {

            if (!File.Exists(settingsVM.VM_Paths.FGPath + @"\bin\fgfs.exe"))
            {
                MaterialMessageBox.ShowError("Couldn't locate fgfs.exe in FlightGear installation folder.");
                return;
            }
            int pn;
            try
            {
                pn = Int32.Parse(portnum.Text);
                if (pn < 1024 || pn > 65535)
                {
                    MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535.");
                    return;
                }
                settingsVM.VM_DestPort = pn;
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid input for port number.");
                return;
            }
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
                if (results == MessageBoxResult.OK)
                {
                    foreach (var process in pname)
                        process.Kill();
                }
                return;
            }
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(settingsVM.VM_Paths.FGPath + @"\bin\fgfs.exe", "--generic=socket,in,10,127.0.0.1," + pn + ",tcp,playback_small --fdm=null --timeofday=morning");
                psi.WorkingDirectory = settingsVM.VM_Paths.FGPath + @"\data";
                Process.Start(psi);

            }
            catch (Exception)
            {
                MaterialMessageBox.ShowError("An error has occured, please make sure that FlightGear is installed properly.");
            }
            settingsVM.EndClient();
            settingsVM.StartClient();


        }

        private void ApplyCSV_Click(object sender, RoutedEventArgs e)
        {
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
            settingsVM.VM_Paths.NormalCSVPath = normalFlightPath.Text;
            settingsVM.VM_Paths.NewCSVPath = newFlightPath.Text;
            reset();
            MaterialMessageBox.Show("new files loaded succesfully");
            normalFlightPath.Text = "";
            newFlightPath.Text = "";
        }
        private void ApplyDLL_Click(object sender, RoutedEventArgs e)
        {
            if (anomalyDetPath.Text == "" || !File.Exists(anomalyDetPath.Text))
            {
                MaterialMessageBox.ShowError("Please select anomaly detection dll.");
                return;
            }
            if (thresholdText.Text == "")
            {
                MaterialMessageBox.ShowError("Please choose a correlation threshold.");
                return;
            }

            float threshold;
            try
            {
                threshold = float.Parse(thresholdText.Text);
                if (threshold <= 0 || threshold >= 1)
                {
                    MaterialMessageBox.ShowError("Threshold value must be larger than 0 and smaller than 1.");
                    return;
                }
                settingsVM.VM_Threshold = threshold;
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid input for threshold value. Please enter a number between 0-1.");
                return;
            }

            settingsVM.VM_Paths.DLLPath = anomalyDetPath.Text;            
            reset();
            MaterialMessageBox.Show("plugin loaded succesfully");
            anomalyDetPath.Text = "";
            thresholdText.Text = "";
        }

        private void ApplyPort_Click(object sender, RoutedEventArgs e)
        {
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

            settingsVM.EndClient();
            settingsVM.VM_DestPort = pn;
            if(settingsVM.StartClient())
                MaterialMessageBox.Show("Connected to new port");
            else
                MaterialMessageBox.ShowError("Failed to connect to new port. Please try again.");
            portnum.Text = "";
        }
        private void reset()
        {
            settingsVM.Reset();
            settingsVM.LoadData();
            settingsVM.AnalyzeData();
        }

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
