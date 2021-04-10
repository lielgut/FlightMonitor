using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class SettingsViewModel : ViewModel
    {
        public SettingsViewModel(IFlightControl fc) : base(fc) { }
    }
}
