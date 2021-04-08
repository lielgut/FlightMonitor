using BespokeFusion;
using ex1.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ex1.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
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
        public SettingsView()
        {
            InitializeComponent();
            DataContext = settingsVM;
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

            if (!File.Exists(settingsVM.fc.Paths.FGPath + @"\bin\fgfs.exe"))
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
                settingsVM.fc.changePort(pn);
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                return;
            }
            Process[] pname = Process.GetProcessesByName("fgfs");
            if (pname.Length > 0)
            {
                CustomMaterialMessageBox msg = new CustomMaterialMessageBox
                {
                    TxtTitle = { Text = "Warning", Foreground = Brushes.White },
                    TxtMessage = { Text = "We've detected that a FlightGear process (fgfs.exe) is already running, press \"Cancel\" and close the program or click \"Kill FG\"", Foreground = Brushes.Black },
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
                ProcessStartInfo psi = new ProcessStartInfo(settingsVM.fc.Paths.FGPath + @"\bin\fgfs.exe", "--generic=socket,in,10,127.0.0.1," + portnum + ",tcp,playback_small --fdm=null");
                psi.WorkingDirectory = settingsVM.fc.Paths.FGPath + @"\data";
                Process.Start(psi);

            }
            catch (Exception)
            {
                MaterialMessageBox.ShowError("An error has occured, please make sure that FlightGear is installed properly.");
            }
            settingsVM.fc.endClient();
            settingsVM.fc.startClient();


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
            settingsVM.fc.Paths.NormalCSVPath = normalFlightPath.Text;
            settingsVM.fc.Paths.NewCSVPath = newFlightPath.Text;
            reset();
        }
        private void ApplyDLL_Click(object sender, RoutedEventArgs e)
        {
            if (anomalyDetPath.Text == "" || !File.Exists(anomalyDetPath.Text))
            {
                MaterialMessageBox.ShowError("Please select anomaly detection dll.");
                return;
            }
            settingsVM.fc.Paths.DLLPath = anomalyDetPath.Text;
            reset();

        }

        private void ApplyPort_Click(object sender, RoutedEventArgs e)
        {
            settingsVM.fc.endClient();
            try
            {
                int pn = Int32.Parse(portnum.Text);
                if (pn < 1024 || pn > 65535)
                {
                    MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                    return;
                }
                settingsVM.fc.changePort(pn);
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid port number.\r\nPlease enter a port between 1024-65535");
                return;
            }
            settingsVM.fc.startClient();
        }
        private void reset()
        {
            settingsVM.fc.reset();
            settingsVM.fc.loadData(settingsVM.fc.Paths.NewCSVPath);
            settingsVM.fc.analyzeData(settingsVM.fc.Paths.NormalCSVPath, settingsVM.fc.Paths.NewCSVPath, settingsVM.fc.Paths.DLLPath);

        }

    }
}
