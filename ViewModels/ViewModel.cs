using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ex1.Model;

namespace ex1.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {
        protected IFlightControl Model;
        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel(IFlightControl model) 
        {
            this.Model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
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
