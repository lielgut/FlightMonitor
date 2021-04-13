using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;
using ex1.Model;
using OxyPlot.Wpf;

namespace ex1.ViewModels
{
    // the ResearchViewModel is responsible for the presentation logic of the research view
    class ResearchViewModel : ViewModel
    {
        // view model constructor (contains the model)
        public ResearchViewModel(IFlightControl model) : base(model) { }

        // currently selected feature from the list
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
                // notify selected feature and its correlative feature have changed
                PropertyChangedNotify("VM_SelectedFeature");
                PropertyChangedNotify("VM_CorrFeature");
            }
        }

        // correlative feature to the selected feature
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

        // list of features
        public List<String> VM_Features
        {
            get
            {
                return Model.getFeaturesList();
            }            
        }

        // list of timesteps with anomalies
        public List<int> VM_AnomaliesList
        {
            get
            {
                return Model.getAnomaliesList(selectedFeature);
            }
        }

        // list of selected feature's DataPoints for graph
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

        // list of DataPoints for graph of the correlative feature
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

        // list of ScatterPoints for the graph of the recent 30 seconds
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

        // list of ScatterPoints of anomalies from the recent 30 seconds
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

        // annotation to be presented in the graph
        public Annotation VM_Annotation
        {
            get
            {
                return Model.getFeatureAnnotation(VM_SelectedFeature);
            }
        }

        // minimal value for X-Axis
        public double VM_MinX
        {
            get
            {
                return Model.getMinX(VM_SelectedFeature);
            }
        }

        // maximal value for X-Axis
        public double VM_MaxX
        {
            get
            {
                return Model.getMaxX(VM_SelectedFeature);
            }
        }

        // minimal value for Y-Axis
        public double VM_MinY
        {
            get
            {
                return Model.getMinY(VM_SelectedFeature);
            }
        }

        // maximal value for Y-Axis
        public double VM_MaxY
        {
            get
            {
                return Model.getMaxY(VM_SelectedFeature);
            }
        }

        // current timestep of the flight
        public int VM_CurrTimestep
        {
            set
            {
                Model.Timestep = value;
            }
        }

        // update current frame in FlightGear
        public void update()
        {
            Model.SendCurrentData();
        }

    }
}
