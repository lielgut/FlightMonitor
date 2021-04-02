using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using ex1.Model;

namespace ex1.ViewModels
{
    class ResearchViewModel : ViewModel
    {
        public ResearchViewModel(IFlightControl fc) : base(fc) { }

        private String selectedFeature;
        public String VM_SelectedFeature
        {
            get
            {
                return selectedFeature;
            }
            set
            {
                selectedFeature = value;
                PropertyChangedNotify("VM_SelectedFeature");
                PropertyChangedNotify("VM_CorrFeature");
            }
        }

        public String VM_CorrFeature
        {
            get
            {
                return fc.getCorrelative(selectedFeature);
            }
        }
        
        public PlotModel VM_Plot
        {
            get
            {
                return fc.getCurrentPlot(selectedFeature);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<String> VM_Features
        {
            get
            {
                return fc.getFeaturesList();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
