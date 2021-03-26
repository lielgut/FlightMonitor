using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class MainViewModel : ViewModel
    {       
        public MainViewModel(IFlightControl fc) : base(fc) { }
    }
}
