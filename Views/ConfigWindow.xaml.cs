using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using ex1.Model;
using BespokeFusion;
using System.Threading;

namespace ex1.Views
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        // private MainWindow mw;
        private IFlightControl fc;
        volatile bool done = false;

        public ConfigWindow()
        {
            this.fc = new FlightControl();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (fgPath.Text == "")
            {
                MaterialMessageBox.ShowError("Please select FlightGear installation folder");
                return;
            }
            if (!File.Exists(fgPath.Text + "//data//Protocol//playback_small.xml"))
            {
                MaterialMessageBox.ShowError("playback_small.xml not found in FlightGear directory.\r\nPlease add the file to data/Protocol folder");
                return;
            }

            if (normalFlightPath.Text == "" || !File.Exists(normalFlightPath.Text))
            {
                MaterialMessageBox.ShowError("Please select normal flight CSV file");
                return;
            }

            if (newFlightPath.Text == "" || !File.Exists(newFlightPath.Text))
            {
                MaterialMessageBox.ShowError("Please select new flight CSV file");
                return;
            }

            if (anomalyDetPath.Text == "" || !File.Exists(anomalyDetPath.Text))
            {
                MaterialMessageBox.ShowError("Please select anomaly detection dll");
                return;
            }

            try
            {
                int pn = Int32.Parse(portnum.Text);
                if (pn < 1024 || pn > 65535)
                {
                    MaterialMessageBox.ShowError("Invalid port number");
                    return;
                }
                fc.changePort(pn);
            }
            catch (System.FormatException)
            {
                MaterialMessageBox.ShowError("Invalid port number");
                return;
            }

            if (!fc.startClient())
            {
                MaterialMessageBox.ShowError("Server is inactive at specified port.\r\nPlease wait for FlightGear server to load or check settings");
                return;
            }

            done = true;

            Thread t = new Thread(delegate ()
           {
               /*CustomMaterialMessageBox m = new CustomMaterialMessageBox();
               m.Title = "bla bla";
               m.Show();*/
               while (!done) { System.Diagnostics.Debug.WriteLine("waiting..."); }
               //m.Close()
               //;
           });
            t.Start();
            

            fc.loadFeatures("..//..//..//playback_small.xml");
            fc.loadData(newFlightPath.Text);
            fc.analyzeData(normalFlightPath.Text, newFlightPath.Text, anomalyDetPath.Text);

            done = true;
            t.Join();

            MainWindow mw = new MainWindow(fc);
            mw.Show();
            this.Close();

            // mw.Visibility = Visibility.Visible;
        }

        private void portnum_TextChanged(object sender, TextChangedEventArgs e)
        {
            fgSetting.Text = "--generic=socket,in,10,127.0.0.1," + portnum.Text + ",tcp,playback_small\r\n--fdm=null";
            copied.Visibility = Visibility.Hidden;
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(fgSetting.Text);
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
    }
}
