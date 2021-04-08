using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ex1.ViewModels;
using ex1.Model;
using ex1.Views;

namespace ex1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IFlightControl fc;
        ResearchView researchView;
        SettingsView settingsView;
        internal MainWindow(IFlightControl fc)
        {
            this.fc = fc;
            // ConfigWindow cw = new ConfigWindow(this, fc);
            // cw.Show();
            InitializeComponent();

            PlayerViewModel pvm = new PlayerViewModel(fc); ;
            playerView.PlayerVM = pvm;
            playerView.DataContext = pvm;

            DataViewModel dvm = new DataViewModel(fc);
            dataView.DataVM = dvm;
            dataView.DataContext = dvm;

            ResearchViewModel rvm = new ResearchViewModel(fc);
            researchView = new ResearchView();
            researchView.ResearchVM = rvm;
            researchView.DataContext = rvm;

            SettingsViewModel svm = new SettingsViewModel(fc);
            settingsView = new SettingsView();
            settingsView.SettingsVM = svm;
            selectedView.DataContext = rvm;

            selectedView.Content = dataView;
        }

        private void TabSelected(object sender, RoutedEventArgs e)
        {
            TabItem selectedTab = sender as TabItem;
            if(selectedTab != null)
            {
                switch(selectedTab.Name)
                {
                    case "dataTab":
                        selectedView.Content = dataView;
                        playerView.Visibility = Visibility.Visible;
                        break;
                    case "researchTab":
                        selectedView.Content = researchView;
                        playerView.Visibility = Visibility.Visible;
                        break;
                    case "settingsTab":
                        selectedView.Content = settingsView;
                        playerView.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }
    }
}