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
        public MainWindow()
        {
            InitializeComponent();
            IFlightControl fc = new FlightControl();
            ConfigWindow cw = new ConfigWindow(this,fc);
            cw.Show();           
            playerView.PlayerVM = new PlayerViewModel(fc);
            dataView.DataVM = new DataViewModel(fc);
            reserachView.ResearchVM = new ResearchViewModel(fc);
            Visibility = Visibility.Hidden;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //fc.endClient(); 
        }

    }
}
