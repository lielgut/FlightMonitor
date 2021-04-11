﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ex1.Model;

namespace ex1.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {
        protected IFlightControl fc;
        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel(IFlightControl fc) 
        {
            this.fc = fc;
            fc.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                PropertyChangedNotify("VM_" + e.PropertyName);
            };
        }
        public void PropertyChangedNotify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
