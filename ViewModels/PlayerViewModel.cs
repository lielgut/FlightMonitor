using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class PlayerViewModel : ViewModel
    {
        public float VM_Speed
        {
            get { return fc.Speed; }
            set { fc.Speed = value;  }
        }
        public int VM_Timestep
        {
            get { return fc.Timestep; }
            set { fc.Timestep = value; }
        }
        public PlayerViewModel(IFlightControl fc) : base(fc) { }

    }
}
