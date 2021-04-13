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
    // window for neccesary configurations at program startup
    public partial class ConfigWindow : Window
    {
        // the settings view model is used for applying the configurations
        private SettingsViewModel _settingsVM;
        
        // constructor for the window
        public ConfigWindow()
        {
            // initialize the settings view model with a new model
            _settingsVM = new SettingsViewModel(new FlightControl());
            InitializeComponent();
        }

        // event for when the done button is clicked
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {

            // verify that a FG installation path was entered
            if (fgPath.Text == "")
            {
                MaterialMessageBox.ShowError("Please select FlightGear installation folder.");
                return;
            }

            // verify that the required xml file is found in the FG installation folder
            if (!File.Exists(fgPath.Text + "//data//Protocol//playback_small.xml"))
            {
                MaterialMessageBox.ShowError("playback_small.xml not found in FlightGear directory.\r\nPlease add the file to data/Protocol folder");
                return;
            }

            // verify CSV files paths
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

            // verify dll plugin path
            if (anomalyDetPath.Text == "" || !File.Exists(anomalyDetPath.Text))
            {
                MaterialMessageBox.ShowError("Please select anomaly detection dll.");
                return;
            }

            // verify that a threhsold was entered
            if (thresholdText.Text == "")
            {
                MaterialMessageBox.ShowError("Please choose a correlation threshold.");
                return;
            }

            float threshold;
            // set the threshold if it is valid
            try
            {                
                threshold = float.Parse(thresholdText.Text);
                if(threshold <= 0 || threshold >= 1)
                {
                    MaterialMessageBox.ShowError("Threshold value must be larger than 0 and smaller than 1.");
                    return;
                }
                _settingsVM.VM_Threshold = threshold;
            }            
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid input for threshold value. Please enter a number between 0-1.");
                return;
            }

            // set the port if it is valid
            try
            {
                int pn = Int32.Parse(portnum.Text);
                if (pn < 1024 || pn > 65535)
                {
                    MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                    return;
                }
                _settingsVM.VM_DestPort = pn;
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                return;
            }

            // verify that FlightGear is running
            Process[] pname = Process.GetProcessesByName("fgfs");
            if (pname.Length == 0)
            {
                MaterialMessageBox.ShowError("FlightGear isn't running.");
                return;
                
            }

            // start client and connect to server, show error if connection failed
            if (!_settingsVM.StartClient())
            {
                MaterialMessageBox.ShowError("Server is inactive at specified port.\r\nPlease wait for FlightGear server to load or check settings");
                return;
            }

            // save all paths
            _settingsVM.VM_Paths.FGPath = fgPath.Text;
            _settingsVM.VM_Paths.NormalCSVPath = normalFlightPath.Text;
            _settingsVM.VM_Paths.NewCSVPath = newFlightPath.Text;
            _settingsVM.VM_Paths.DLLPath = anomalyDetPath.Text;
            _settingsVM.VM_Paths.XMLPath = xmlPath.Text;

            // load feature names from the XML file
            _settingsVM.LoadFeatures();
            // load data from the CSV file
            _settingsVM.LoadData();
            // analyze the data with loaded plugin
            _settingsVM.AnalyzeData();

            // create and show the main window
            _settingsVM.LoadMainWindow().Show();
            this.Close();

        }

        // if a port number was entered, change the textbox text to allow the user to copy the settings to FG
        private void portnum_TextChanged(object sender, TextChangedEventArgs e)
        {
            fgSetting.Text = "--generic=socket,in,10,127.0.0.1," + portnum.Text + ",tcp,playback_small\r\n--fdm=null --timeofday=morning";
            copied.Visibility = Visibility.Hidden;
        }

        // event for clicking the copy button which allows the user to copy the settings from the textbox
        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(fgSetting.Text);
            copied.Visibility = Visibility.Visible;
        }

        // browse to find the FlightGear installation folder
        private void BrowseFgPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog browser = new System.Windows.Forms.FolderBrowserDialog();
            browser.ShowDialog();
            fgPath.Text = browser.SelectedPath;
        }

        // browse to find the normal flight CSV
        private void BrowseNormal_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "CSV file (*.csv)|*.csv";
            browser.ShowDialog();
            normalFlightPath.Text = browser.FileName;
        }

        // browse to find the new flight CSV
        private void BrowseNew_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "CSV file (*.csv)|*.csv";
            browser.ShowDialog();
            newFlightPath.Text = browser.FileName;
        }

        // browse to find the plugin DLL file
        private void BrowseAnomaly_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "Plugin file (*.dll)|*.dll";
            browser.ShowDialog();
            anomalyDetPath.Text = browser.FileName;
        }

        // browse to find the XML file with feature names
        private void BrowseXml_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.Filter = "Xml file (*.xml)|*.xml";
            browser.ShowDialog();
            xmlPath.Text = browser.FileName;
        }

        // event for when the "launch FlightGear" button is clicked
        private void LaunchFG_Click(object sender, RoutedEventArgs e)
        {
            // verify FG installation path
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

            // verify port number
            try
            {
                int pn = Int32.Parse(portnum.Text);
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

            // verify FlightGear isn't already running
            Process[] pname = Process.GetProcessesByName("fgfs");
            if (pname.Length > 0)
            {
                MaterialMessageBox.ShowError("FlightGear is already running! please close it and try again.");
                return;
            }
            // try to start FlightGear
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
