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
                return fc.getCurrentData("throttle");
            }
        }
        public float VM_Rudder
        {
            get
            {
                return fc.getCurrentData("rudder");
            }
        }
        public float VM_Aileron
        {
            get
            {
                return fc.getCurrentData("aileron");
            }
        }
        public float VM_Elevator
        {
            get
            {
                return fc.getCurrentData("elevator");
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
