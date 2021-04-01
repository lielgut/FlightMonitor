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

        private PlotModel plot;
        public PlotModel VM_Plot
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
