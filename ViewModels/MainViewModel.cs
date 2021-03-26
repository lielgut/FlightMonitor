using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class MainViewModel : ViewModel
    {
        private IFlightControl fc;

        public MainViewModel()
        {
            fc = new FlightControl();
        }
    }
}
