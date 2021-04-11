using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using ex1.Model;
using ex1.ViewModels;
using BespokeFusion;
using System.Diagnostics;

namespace ex1.Views
{
    public partial class ConfigWindow : Window
    {
        // private MainWindow mw;
        private SettingsViewModel SettingsVM;
        
        public ConfigWindow()
        {
            this.SettingsVM = new SettingsViewModel(new FlightControl());
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (fgPath.Text == "")
            {
                MaterialMessageBox.ShowError("Please select FlightGear installation folder.");
                return;
            }
            if (!File.Exists(fgPath.Text + "//data//Protocol//playback_small.xml"))
            {
                MaterialMessageBox.ShowError("playback_small.xml not found in FlightGear directory.\r\nPlease add the file to data/Protocol folder");
                return;
            }

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
                if(threshold <= 0 || threshold >= 1)
                {
                    MaterialMessageBox.ShowError("Threshold value must be larger than 0 and smaller than 1.");
                    return;
                }
                SettingsVM.VM_Threshold = threshold;
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid input for threshold value. Please enter a number between 0-1.");
                return;
            }

            try
            {
                int pn = Int32.Parse(portnum.Text);
                if (pn < 1024 || pn > 65535)
                {
                    MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                    return;
                }
                SettingsVM.VM_DestPort = pn;
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                return;
            }
            Process[] pname = Process.GetProcessesByName("fgfs");
            if (pname.Length == 0)
            {
                MaterialMessageBox.ShowError("FlightGear isn't running.");
                return;
                
            }
                if (!SettingsVM.StartClient())
            {
                MaterialMessageBox.ShowError("Server is inactive at specified port.\r\nPlease wait for FlightGear server to load or check settings");
                return;
            }

            SettingsVM.VM_Paths.FGPath = fgPath.Text;
            SettingsVM.VM_Paths.NormalCSVPath = normalFlightPath.Text;
            SettingsVM.VM_Paths.NewCSVPath = newFlightPath.Text;
            SettingsVM.VM_Paths.DLLPath = anomalyDetPath.Text;

            SettingsVM.LoadFeatures("..//..//..//Resources//playback_small.xml");
            SettingsVM.LoadData();
            
            SettingsVM.AnalyzeData();

            SettingsVM.LoadMainWindow().Show();
            this.Close();

        }

        private void portnum_TextChanged(object sender, TextChangedEventArgs e)
        {
            fgSetting.Text = "--generic=socket,in,10,127.0.0.1," + portnum.Text + ",tcp,playback_small\r\n--fdm=null --timeofday=morning";
            copied.Visibility = Visibility.Hidden;
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(fgSetting.Text);
            copied.Visibility = Visibility.Visible;
        }

        private void BrowseFgPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog browser = new System.Windows.Forms.FolderBrowserDialog();
            browser.ShowDialog();
            fgPath.Text = browser.SelectedPath;
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
            if (fgPath.Text == "")
            {
                MaterialMessageBox.ShowError("Please select FlightGear installation folder.");
                return;
            }
            if (!File.Exists(fgPath.Text + @"\bin\fgfs.exe"))
            {
                MaterialMessageBox.ShowError("Couldn't locate fgfs.exe in the FlightGear installation folder.");
                return;
            }
            try
            {
                int pn = Int32.Parse(portnum.Text);
                if (pn < 1024 || pn > 65535)
                {
                    MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                    return;
                }
                SettingsVM.VM_DestPort = pn;
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                return;
            }
            Process[] pname = Process.GetProcessesByName("fgfs");
            if (pname.Length > 0)
            {
                MaterialMessageBox.ShowError("FlightGear is already running!");
                return;
            }
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(fgPath.Text + @"\bin\fgfs.exe", "--generic=socket,in,10,127.0.0.1," + portnum.Text + ",tcp,playback_small --fdm=null --timeofday=morning");
                psi.WorkingDirectory = fgPath.Text + @"\data";
                Process.Start(psi);

            }
            catch(Exception)
            {
                MaterialMessageBox.ShowError("An error has occured, please make sure that FlightGear is installed properly.");
            }
            
        }

    }
}
