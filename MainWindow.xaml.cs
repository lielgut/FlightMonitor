using System.Windows;
using System.Windows.Controls;
using ex1.ViewModels;
using ex1.Model;

namespace ex1
{
    // The main window contains each view as a user-control and allows switching views with tab-control
    public partial class MainWindow : Window
    {
        // constructor gets the model in order to create the view models for each view
        internal MainWindow(IFlightControl model)
        {    
            InitializeComponent();

            // create player view model and set its data context
            PlayerViewModel pvm = new PlayerViewModel(model); ;
            playerView.PlayerVM = pvm;
            playerView.DataContext = pvm;

            // create data view model and set its data context
            DataViewModel dvm = new DataViewModel(model);
            dataView.DataVM = dvm;
            dataView.DataContext = dvm;

            // create research view model and set its data context
            ResearchViewModel rvm = new ResearchViewModel(model);
            researchView.ResearchVM = rvm;
            researchView.DataContext = rvm;

            // create settings view model and set its data context
            SettingsViewModel svm = new SettingsViewModel(model);
            settingsView.SettingsVM = svm;
            settingsView.DataContext = svm;
        }

        // event for when a new tab is selected
        private void TabSelected(object sender, RoutedEventArgs e)
        {
            TabItem selectedTab = sender as TabItem;
            if(selectedTab != null)
            {
                // hide the player view if the settings tab was selected
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