using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using OxyPlot.Series;
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
                if(selectedFeature == null)
                {
                    return null;
                }
                String corr = fc.getCorrelative(selectedFeature);
                if(corr == null)
                {
                    return "no correlative feature";
                }
                return corr;
            }
        }               

        public List<String> VM_Features
        {
            get
            {
                return fc.getFeaturesList();
            }            
        }

        public List<DataPoint> VM_FeaturePoints
        {
            get
            {
                List<DataPoint> l = fc.getDataPoints(selectedFeature);
                if(l == null)
                {
                    return null;
                }
                return l;
            }
        }

        public List<DataPoint> VM_SecondFeaturePoints
        {
            get
            {
                String corrFeature = VM_CorrFeature;
                if(corrFeature == null || corrFeature == "no correlative feature")
                {
                    return null;
                }
                List<DataPoint> l = fc.getDataPoints(corrFeature);
                if (l == null)
                {
                    return null;
                }
                return new List<DataPoint>(l);
            }
        }

        public List<ScatterPoint> VM_CorrFeaturesPoints
        {
            get
            {
                if (VM_CorrFeature == null || VM_CorrFeature == "no correlative feature")
                {
                    return null;
                }        
                return fc.getRecentScatterPoints(selectedFeature);
            }
        }

    }
}
