using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class MainViewModel : ViewModel
    {
        private DataViewModel dataVM;
        private PlayerViewModel playerVM;
        private ResearchViewModel researchVM;
        public MainViewModel(IFlightControl fc) : base(fc)
        {
            this.dataVM = new DataViewModel(fc);
            this.playerVM = new PlayerViewModel(fc);
            this.researchVM = new ResearchViewModel(fc);
        }
    }
}
