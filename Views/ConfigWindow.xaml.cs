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

namespace ex1.Views
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        // private MainWindow mw;
        private IFlightControl fc;

        public ConfigWindow()
        {
            this.fc = new FlightControl();
            InitializeComponent();                                    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
             if(fgPath.Text == "")
            {
                MessageBox.Show("Please select FlightGear installation folder.");
                return;
            }
             if(!File.Exists(fgPath.Text + "//data//Protocol//playback_small.xml"))
            {
                MessageBox.Show("playback_small.xml not found in FlightGear directory.\r\nPlease add the file to data/Protocol folder.");
                return;
            }

            if (normalFlightPath.Text == "" || !File.Exists(normalFlightPath.Text))
            {
                MessageBox.Show("Please select normal flight CSV file.");
                return;
            }

            if (newFlightPath.Text == "" || !File.Exists(newFlightPath.Text))
            {
                MessageBox.Show("Please select new flight CSV file.");
                return;
            }                        
            
            if (anomalyDetPath.Text == "" || !File.Exists(anomalyDetPath.Text))
            {
                MessageBox.Show("Please select anomaly detection dll.");
                return;
            }

            try
            {
                int pn = Int32.Parse(portnum.Text);
                if(pn < 1024 || pn > 65535)
                {
                    MessageBox.Show("Invalid port number.");
                    return;
                }
                fc.changePort(pn);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Invalid port number.");
                return;
            }

            if(!fc.startClient())
            {
                MessageBox.Show("Server is inactive at specified port.\r\nPlease wait for FlightGear server to load or check settings.");
                return;
            }
            fc.loadFeatures("..//..//..//playback_small.xml");
            fc.loadData(newFlightPath.Text);

            // fc.start();

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
            browser.ShowDialog();
            normalFlightPath.Text = browser.FileName;
        }

        private void BrowseNew_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.ShowDialog();
            newFlightPath.Text = browser.FileName;
        }

        private void BrowseAnomaly_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog browser = new System.Windows.Forms.OpenFileDialog();
            browser.ShowDialog();
            anomalyDetPath.Text = browser.FileName;            
        }
    }
}
