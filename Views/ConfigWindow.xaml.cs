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
        private MainWindow mw;
        private IFlightControl fc;

        internal ConfigWindow(MainWindow mw, IFlightControl fc)
        {
            this.mw = mw;
            this.fc = fc;
            InitializeComponent();                        
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            

            try
            {
                fc.changePort(Int32.Parse(portnum.Text));
            }
            catch(System.FormatException)
            {
                MessageBox.Show("Invalid port number.");
                return;
            }
            if(fgPath.Text == "")
            {
                MessageBox.Show("Please select FlightGear installation folder.");
                return;
            }

            

            this.Close();            
            mw.Visibility = Visibility.Visible;
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
            System.Windows.Forms.FolderBrowserDialog browser = new System.Windows.Forms.FolderBrowserDialog();
            browser.ShowDialog();
            normalFlightPath.Text = browser.SelectedPath;
        }

        private void BrowseNew_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog browser = new System.Windows.Forms.FolderBrowserDialog();
            browser.ShowDialog();
            newFlightPath.Text = browser.SelectedPath;
        }

        private void BrowseAnomaly_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog browser = new System.Windows.Forms.FolderBrowserDialog();
            browser.ShowDialog();
            anomalyDetPath.Text = browser.SelectedPath;
        }
    }
}
