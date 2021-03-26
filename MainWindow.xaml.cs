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
        ViewModel vm;
        public MainWindow()
        {
            //ConfigWindow cw = new ConfigWindow();
            //cw.Show();
            InitializeComponent();            
            vm = new MainViewModel(new FlightControl());
            DataContext = vm;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void DataView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
