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
using OxyPlot.Wpf;

namespace ex1.Views
{
    /// <summary>
    /// Interaction logic for ResearchView.xaml
    /// </summary>
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
            featuresPoints.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 10, Position = OxyPlot.Axes.AxisPosition.Bottom });
            featuresPoints.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 10, Position = OxyPlot.Axes.AxisPosition.Left });
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selected = (sender as ListBox).SelectedItem as String;
            researchVM.VM_SelectedFeature = selected;

            Annotation a = researchVM.fc.getFeatureAnnotation(selected);
            featuresPoints.Annotations.Clear();
            if (a != null)
            {
                a.Layer = OxyPlot.Annotations.AnnotationLayer.BelowSeries;                
                featuresPoints.Annotations.Add(a);

                double min = Math.Min(researchVM.fc.getMinX(selected), researchVM.fc.getMinY(selected));
                double max = Math.Max(researchVM.fc.getMaxX(selected), researchVM.fc.getMaxY(selected));
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
                researchVM.fc.Timestep = (int) selectedTime;
        }
    }
}
