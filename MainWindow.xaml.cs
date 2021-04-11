﻿using System;
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
        internal MainWindow(IFlightControl fc)
        {    
            InitializeComponent();

            PlayerViewModel pvm = new PlayerViewModel(fc); ;
            playerView.PlayerVM = pvm;
            playerView.DataContext = pvm;

            DataViewModel dvm = new DataViewModel(fc);
            dataView.DataVM = dvm;
            dataView.DataContext = dvm;

            ResearchViewModel rvm = new ResearchViewModel(fc);
            researchView.ResearchVM = rvm;
            researchView.DataContext = rvm;

            SettingsViewModel svm = new SettingsViewModel(fc);
            settingsView.SettingsVM = svm;
            settingsView.DataContext = svm;
        }

        private void TabSelected(object sender, RoutedEventArgs e)
        {
            TabItem selectedTab = sender as TabItem;
            if(selectedTab != null)
            {
                switch(selectedTab.Name)
                {
                    case "dataTab":
                    case "researchTab":
                        playerView.Visibility = Visibility.Visible;
                        break;
                    case "settingsTab":
                        playerView.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }
    }
}