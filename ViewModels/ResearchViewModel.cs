using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;
using ex1.Model;
using OxyPlot.Wpf;

namespace ex1.ViewModels
{
    class ResearchViewModel : ViewModel
    {
        public ResearchViewModel(IFlightControl model) : base(model) { }

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
                String corr = Model.getCorrelative(selectedFeature);
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
                return Model.getFeaturesList();
            }            
        }

        public List<int> VM_AnomaliesList
        {
            get
            {
                return Model.getAnomaliesList(selectedFeature);
            }
        }

        public List<DataPoint> VM_FeaturePoints
        {
            get
            {
                List<DataPoint> l = Model.getDataPoints(selectedFeature);
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
                List<DataPoint> l = Model.getDataPoints(corrFeature);
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
                return Model.getRecentScatterPoints(selectedFeature);
            }
        }

        public List<ScatterPoint> VM_AnomalousPoints
        {
            get
            {
                if (VM_CorrFeature == null || VM_CorrFeature == "no correlative feature")
                {
                    return null;
                }
                return Model.getRecentAnomalousPoints(selectedFeature);
            }
        }

        public Annotation VM_Annotation
        {
            get
            {
                return Model.getFeatureAnnotation(VM_SelectedFeature);
            }
        }

        public double VM_MinX
        {
            get
            {
                return Model.getMinX(VM_SelectedFeature);
            }
        }

        public double VM_MaxX
        {
            get
            {
                return Model.getMaxX(VM_SelectedFeature);
            }
        }

        public double VM_MinY
        {
            get
            {
                return Model.getMinY(VM_SelectedFeature);
            }
        }

        public double VM_MaxY
        {
            get
            {
                return Model.getMaxY(VM_SelectedFeature);
            }
        }

        public int VM_CurrTimestep
        {
            set
            {
                Model.Timestep = value;
            }
        }

        public void update()
        {
            Model.SendCurrentData();
        }

    }
}
