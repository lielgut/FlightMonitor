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
using ex1.ViewModels;

namespace ex1.Views
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : UserControl
    {
        private DataViewModel dataVM;
        internal DataViewModel DataVM
        {
            get
            {
                return dataVM;
            }
            set
            {
                dataVM = value;
            }
        }
        public DataView()
        {
            InitializeComponent();
            DataContext = dataVM;
        }
    }
}
