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
                PropertyChangedNotify("VM_Plot");
            }
        }

        public String VM_CorrFeature
        {
            get
            {
                if(selectedFeature == null)
                {
                    return "null";
                }
                String corr = fc.getCorrelative(selectedFeature);
                if(corr == null)
                {
                    return "no correlative feature";
                }
                return corr;
            }
        }
        
        public PlotModel VM_Plot
        {
            get
            {          
                if(selectedFeature == null)
                {
                    return null;
                }
                return fc.getCurrentPlot(selectedFeature);
            }
        }

        public List<String> VM_Features
        {
            get
            {
                return fc.getFeaturesList();
            }            
        }
    }
}
