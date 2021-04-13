using ex1.Model;

namespace ex1.ViewModels
{
    // the PlayerViewModel is responsible for the presentation logic of the player view
    class PlayerViewModel : ViewModel
    {
        // view model constructor (contains the model)
        public PlayerViewModel(IFlightControl model) : base(model) { }

        // is the player currently playing
        public bool VM_IsPlaying
        {
            get { return !Model.Stop; }
            set { Model.Stop = !value; }
        }

        // playback speed of the player
        public float VM_Speed
        {
            get { return Model.Speed; }
            set { Model.Speed = value;  }
        }

        // is the player currently playing in reverse
        public bool VM_IsReverse
        {
            get { return Model.IsReverse; }
            set { Model.IsReverse = value; }
        }

        // current timestep of the flight
        public int VM_Timestep
        {
            get { return Model.Timestep; }
            set { Model.Timestep = value; }
        }

        // current timestep converted to minutes
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

        // max value for player slider
        public int VM_Length
        {
            get { return Model.NumLines - 1; }
        }

        // max timestep converted to minutes
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

        // icon to be presented on the play button (play/pause)
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

        // start the player
        public void start()
        {
            Model.start();
        }

        // stop the player
        public void stop()
        {
            Model.stop();
        }

        // update current frame in FlightGear
        public void update()
        {
            Model.SendCurrentData();
        }      

    }
}
