using System;
using System.Collections.Generic;
using System.Text;
using ex1.Model;

namespace ex1.ViewModels
{
    class PlayerViewModel : ViewModel
    {
        public PlayerViewModel(IFlightControl model) : base(model) { }

        public bool VM_IsPlaying
        {
            get { return !Model.Stop; }
            set { Model.Stop = !value; }
        }
        public float VM_Speed
        {
            get { return Model.Speed; }
            set { Model.Speed = value;  }
        }
        public bool VM_IsReverse
        {
            get { return Model.IsReverse; }
            set { Model.IsReverse = value; }
        }
        public int VM_Timestep
        {
            get { return Model.Timestep; }
            set { Model.Timestep = value; }
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
            get { return Model.NumLines - 1; }
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

        public MaterialDesignThemes.Wpf.PackIconKind VM_PlayIcon
        {
            get
            {
                if(Model.Stop)
                {
                    return MaterialDesignThemes.Wpf.PackIconKind.Play;
                }
                else
                {
                    return MaterialDesignThemes.Wpf.PackIconKind.Pause;
                }
            }
        }

        public void start()
        {
            Model.start();
        }

        public void stop()
        {
            Model.stop();
        }

        public void update()
        {
            Model.SendCurrentData();
        }      

    }
}
