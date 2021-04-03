using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class PlayerViewModel : ViewModel
    {
        public bool VM_IsPlaying
        {
            get { return !fc.Stop; }
            set { fc.Stop = !value; }
        }
        public float VM_Speed
        {
            get { return fc.Speed; }
            set { fc.Speed = value;  }
        }
        public bool VM_IsReverse
        {
            get { return fc.IsReverse; }
            set { fc.IsReverse = value; }
        }
        public int VM_Timestep
        {
            get { return fc.Timestep; }
            set { fc.Timestep = value; }
        }
        public string VM_Minute
        {
            get
            {
                int ts = VM_Timestep;
                int seconds = (int)(0.1f * (float)ts);
                int minutes = seconds / 60;
                seconds %= 60;
                string sMinutes = minutes >= 10 ? minutes.ToString() : "0" + minutes.ToString();
                string sSeconds = seconds >= 10 ? seconds.ToString() : "0" + seconds.ToString();
                return sMinutes + ":" + sSeconds;
            }
        }
        public int VM_Length
        {
            get { return fc.NumLines - 1; }
        }
        public string VM_MaxTime
        {
            get {
                int ts = VM_Length;
                int seconds = (int)(0.1f * (float)ts);
                int minutes = seconds / 60;
                seconds %= 60;
                string sMinutes = minutes >= 10 ? minutes.ToString() : "0" + minutes.ToString();
                string sSeconds = seconds >= 10 ? seconds.ToString() : "0" + seconds.ToString();
                return sMinutes + ":" + sSeconds;
            }
        }
        public PlayerViewModel(IFlightControl fc) : base(fc) { }

    }
}
