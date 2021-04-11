using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class DataViewModel : ViewModel
    {
        public DataViewModel(IFlightControl model) : base(model) { }

        public float VM_Throttle
        {
            get
            {
                // -120 for going up in the Y axis, the values of throttle are 0-1.
                return (float)Math.Round(Model.getCurrentData("throttle")*(-120),2);
            }
        }
        public float VM_Rudder
        {
            get
            {
                // I assume the data is 0-1, max value we can achieve is 120, min is 0.
                return (float)Math.Round(Model.getCurrentData("rudder")*120,2);
            }
        }
        public float VM_Aileron
        {
            get
            {
                return (float)Math.Round(Model.getCurrentData("aileron")*80,2);
            }
        }
        public float VM_Elevator
        {
            get
            {
                return (float)Math.Round(Model.getCurrentData("elevator")*80,2);
            }
        }
        public float VM_Altimeter
        {
            get
            {
                return (float)Math.Round(Model.getCurrentData("altimeter_indicated-altitude-ft"),2);
            }
        }
        public float VM_Airspeed
        {
            get
            {
                return (float)Math.Round(Model.getCurrentData("airspeed-kt"),2);
            }
        }
        public float VM_SpeedToAngle
        {
            get
            {
                float newSpeed = VM_Airspeed * 1.4f -140f;
                if (newSpeed > 140f)
                    return 140f;
                return newSpeed;
            }
        }
        public float VM_HeadingDeg
        {
            get
            {
                return (float)Math.Round(Model.getCurrentData("heading-deg"),2);
            }
        }
        public float VM_PitchDeg
        {
            get
            {
                return (float)Math.Round(Model.getCurrentData("pitch-deg"),2);
            }
        }
        public float VM_RollDeg
        {
            get
            {
                return (float)Math.Round(Model.getCurrentData("roll-deg"),2);
            }
        }
        public float VM_SideSlipDeg
        {
            get
            {
                return (float)Math.Round(Model.getCurrentData("side-slip-deg"),2);
            }
        }             
    }
}
