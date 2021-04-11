using System;
using System.Windows;
using System.Windows.Controls;
using ex1.ViewModels;
using OxyPlot.Wpf;

namespace ex1.Views
{
    public partial class ResearchView : UserControl
    {
        private ResearchViewModel researchVM;
        internal ResearchViewModel ResearchVM
        {
            get
            {                
                return researchVM;
            }
            set
            {
                researchVM = value;
            }
        }
        public ResearchView()
        {
            InitializeComponent();
            DataContext = researchVM;           
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selected = (sender as ListBox).SelectedItem as String;
            researchVM.VM_SelectedFeature = selected;

            Annotation a = researchVM.VM_Annotation;
            featuresPoints.Annotations.Clear();
            if (a != null)
            {
                a.Layer = OxyPlot.Annotations.AnnotationLayer.BelowSeries;                
                featuresPoints.Annotations.Add(a);

                double min = Math.Min(researchVM.VM_MinX, researchVM.VM_MinY);
                double max = Math.Max(researchVM.VM_MaxX, researchVM.VM_MaxY);
                double dist = max - min;
                featuresPoints.Axes[0].Minimum = min - 0.25 * dist;
                featuresPoints.Axes[0].Maximum = max + 0.25 * dist;
                featuresPoints.Axes[1].Minimum = min - 0.25 * dist;
                featuresPoints.Axes[1].Maximum = max + 0.25 * dist;
                featuresPoints.ResetAllAxes();
            }

            researchVM.PropertyChangedNotify("VM_FeaturePoints");
            researchVM.PropertyChangedNotify("VM_AnomaliesList");            
            researchVM.PropertyChangedNotify("VM_SecondFeaturePoints");
            researchVM.PropertyChangedNotify("VM_CorrFeaturesPoints");
            researchVM.PropertyChangedNotify("VM_AnomalousPoints");
        }

        private void Refresh_Clicked(object sender, RoutedEventArgs e)
        {
            listBox_SelectionChanged(listBox, null);
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            featuresPoints.ZoomAllAxes(1.5);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            featuresPoints.ZoomAllAxes(0.5);
        }

        private void anomalies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selectedTime = (sender as ListBox).SelectedItem;
            if(selectedTime != null)
            {
                researchVM.VM_CurrTimestep = (int)selectedTime;
                researchVM.update();
            }                
        }
    }
}
