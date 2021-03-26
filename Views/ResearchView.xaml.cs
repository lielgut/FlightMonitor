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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace ex1.Views
{
    /// <summary>
    /// Interaction logic for ResearchView.xaml
    /// </summary>
    public partial class ResearchView : UserControl
    {
        public ResearchView()
        {
            InitializeComponent();
            ListBoxItem newItem = new ListBoxItem();
            newItem.Content = "Item";
            ListBoxItem newItem1 = new ListBoxItem();
            newItem1.Content = "Item1";
            ListBoxItem newItem2 = new ListBoxItem();
            newItem2.Content = "Item2";
            listBox.Items.Add(newItem);
            listBox.Items.Add(newItem1);
            listBox.Items.Add(newItem2);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = (ListBoxItem) listBox.SelectedItems[0];                   
            MessageBox.Show(lbi.Content.ToString());
            
            
        }
    }
}
