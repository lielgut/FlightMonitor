using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class ResearchViewModel : ViewModel
    {
        public ResearchViewModel(IFlightControl fc) : base(fc) { }        
    }
}
