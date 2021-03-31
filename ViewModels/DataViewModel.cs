using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class DataViewModel : ViewModel
    {
        public float VM_Throttle
        {
            get
            {
                // -120 for going up in the Y axis, the values of throttle are 0-1.
                return fc.getCurrentData("throttle")*(-120);
            }
        }
        public float VM_Rudder
        {
            get
            {
                // I assume the data is 0-1, max value we can achieve is 120, min is 0.
                return fc.getCurrentData("rudder")*120;
            }
        }
        public float VM_Aileron
        {
            get
            {
                return fc.getCurrentData("aileron")*80;
            }
        }
        public float VM_Elevator
        {
            get
            {
                return fc.getCurrentData("elevator")*80;
            }
        }
        public float VM_Altimeter
        {
            get
            {
                return fc.getCurrentData("altimeter_indicated-altitude-ft");
            }
        }
        public float VM_Airspeed
        {
            get
            {
                return fc.getCurrentData("airspeed-kt");
            }
        }
        public float VM_HeadingDeg
        {
            get
            {
                return fc.getCurrentData("heading-deg");
            }
        }
        public float VM_PitchDeg
        {
            get
            {
                return fc.getCurrentData("pitch-deg");
            }
        }
        public float VM_RollDeg
        {
            get
            {
                return fc.getCurrentData("roll-deg");
            }
        }
        public float VM_SideSlipDeg
        {
            get
            {
                return fc.getCurrentData("side-slip-deg");
            }
        }

        public DataViewModel(IFlightControl fc) : base(fc) { }        
    }
}
