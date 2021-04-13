using System.Windows.Controls;
using ex1.ViewModels;

namespace ex1.Views
{
    // the data view visually presents the properties that are found in the data view model
    public partial class DataView : UserControl
    {
        // the data view model is used for gettings values of presented data
        private DataViewModel _dataVM;
        internal DataViewModel DataVM
        {
            get
            {
                return _dataVM;
            }
            set
            {
                _dataVM = value;
            }
        }
        // constructor for the data view
        public DataView()
        {
            InitializeComponent();
        }
    }
}
