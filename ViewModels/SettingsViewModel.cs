﻿using ex1.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ex1.ViewModels
{
    class SettingsViewModel : ViewModel
    {
        public int VM_Port
        {
            get { return fc.getCurrentPort(); }
            set { fc.changePort(value); }
        }

        public SettingsViewModel(IFlightControl fc) : base(fc) { }
    }
}