using System;
using System.Windows;
using System.Windows.Controls;
using ex1.ViewModels;
using OxyPlot.Wpf;

namespace ex1.Views
{
    // view for presenting graphs for selected features
    public partial class ResearchView : UserControl
    {
        // the research view model is used for getting the features and their graphs
        private ResearchViewModel _researchVM;
        internal ResearchViewModel ResearchVM
        {
            get
            {                
                return _researchVM;
            }
            set
            {
                _researchVM = value;
            }
        }

        // constructor for research view
        public ResearchView()
        {
            InitializeComponent();         
        }

        // event for when a new feature was selected from the listbox
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // set selected feature in view model
            String selected = (sender as ListBox).SelectedItem as String;
            _researchVM.VM_SelectedFeature = selected;

            // get annotation for selected feature from view model
            Annotation a = _researchVM.VM_Annotation;
            // clear previous annotation
            featuresPoints.Annotations.Clear();
            if (a != null)
            {
                // set annotation to appear below other graph elements
                a.Layer = OxyPlot.Annotations.AnnotationLayer.BelowSeries; 
                // add the annotation to the graph
                featuresPoints.Annotations.Add(a);

                // set the X,Y axes in order to show the graph properly
                double min = Math.Min(_researchVM.VM_MinX, _researchVM.VM_MinY);
                double max = Math.Max(_researchVM.VM_MaxX, _researchVM.VM_MaxY);
                double dist = max - min;
                featuresPoints.Axes[0].Minimum = min - 0.25 * dist;
                featuresPoints.Axes[0].Maximum = max + 0.25 * dist;
                featuresPoints.Axes[1].Minimum = min - 0.25 * dist;
                featuresPoints.Axes[1].Maximum = max + 0.25 * dist;
                featuresPoints.ResetAllAxes();
            }

            // notify these properties should update
            _researchVM.PropertyChangedNotify("VM_FeaturePoints");
            _researchVM.PropertyChangedNotify("VM_AnomaliesList");            
            _researchVM.PropertyChangedNotify("VM_SecondFeaturePoints");
            _researchVM.PropertyChangedNotify("VM_CorrFeaturesPoints");
            _researchVM.PropertyChangedNotify("VM_AnomalousPoints");
        }

        // event for when the refresh button is clicked
        private void Refresh_Clicked(object sender, RoutedEventArgs e)
        {
            // reload current selected item from listbox
            listBox_SelectionChanged(listBox, null);
        }

        // event for when the zoom in button is clicked
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            // zoom in by a factor of 1.5
            featuresPoints.ZoomAllAxes(1.5);
        }

        // event for when the zoom out button is clicked
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            // zoom out by a factor of 1.5
            featuresPoints.ZoomAllAxes(0.5);
        }

        // event for when a timestep of an anomaly was selected from the listbox
        private void anomalies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // set current time to selected time and update FlightGear
            object selectedTime = (sender as ListBox).SelectedItem;
            if(selectedTime != null)
            {
                _researchVM.VM_CurrTimestep = (int)selectedTime;
                _researchVM.update();
            }                
        }
    }
}
