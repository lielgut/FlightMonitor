using System;
using System.ComponentModel;
using ex1.Model;

namespace ex1.ViewModels
{
    // general view model class
    class ViewModel : INotifyPropertyChanged
    {
        // contains the model
        protected IFlightControl Model;

        // implements the PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        // constructor
        public ViewModel(IFlightControl model) 
        {
            this.Model = model;
            // the view model is an observer of the model
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                PropertyChangedNotify("VM_" + e.PropertyName);
            };
        }

        // notify the view when a property in the view model has changed
        public void PropertyChangedNotify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
