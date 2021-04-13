using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ex1.ViewModels;

namespace ex1.Views
{
    // the player view shows the play button, speed controls and time slider
    public partial class PlayerView : UserControl
    {
        // the player view model is used for getting/setting the time
        private PlayerViewModel _playerVM;        
        internal PlayerViewModel PlayerVM
        {
            get
            {
                return _playerVM;
            }
            set
            {
                _playerVM = value;
            }
        }

        // used for saving the player state       
        private bool wasPaused;

        // constructor for the player view
        public PlayerView()
        {
            InitializeComponent();
        }

        // event for when the play button is clicked
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            // pause if already playing
            if (_playerVM.VM_IsPlaying)
            {
                _playerVM.stop();
            }
            // play if is paused
            else
            {
                // if at the last timestep then reset time
                if (_playerVM.VM_Timestep == _playerVM.VM_Length)
                    _playerVM.VM_Timestep = 0;
                _playerVM.start();
            }
        }

        // event for when the skip 5 sec forward button is clicked
        private void FForward5_Click(object sender, RoutedEventArgs e)
        {
            // skip forward by 5 seconds (50 timesteps) if possible
            if (_playerVM.VM_Timestep + 50 < _playerVM.VM_Length)
                _playerVM.VM_Timestep += 50;
            else
                // otherwise set to the last timestep
                _playerVM.VM_Timestep = _playerVM.VM_Length;
            // update FlightGear
            _playerVM.update();
        }

        // event for when the rewind 5 sec button is clicked
        private void FRewind5_Click(object sender, RoutedEventArgs e)
        {
            // rewind 5 seconds if possible
            if (_playerVM.VM_Timestep - 50 >= 0)
                _playerVM.VM_Timestep -= 50;
            else
                // otherwise reset to start
                _playerVM.VM_Timestep = 0;
            // update FlightGear
            _playerVM.update();
        }

        // double the speed when the fast-forward button is held
        private void FForward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                _playerVM.VM_Speed *= 2f;
            
        }
        // set speed back to normal when mouse is released
        private void FForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _playerVM.VM_Speed /= 2f;
        }

        // double the speed and reverse player when the fast-rewind button is held
        private void FRewind_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _playerVM.VM_IsReverse = true;
            _playerVM.VM_Speed *= 2f;
        }
        // set speed back to normal and stop playing in reverse
        private void FRewind_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _playerVM.VM_IsReverse = false;
            _playerVM.VM_Speed /= 2f;
        }

        // multiply the speed by 4 when the skip-forward button is held
        private void SkipForward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_playerVM.VM_IsPlaying && _playerVM.VM_Timestep != _playerVM.VM_Length)
            {
                _playerVM.start();
            }
            _playerVM.VM_Speed *= 4f;
        }

        // set speed back to normal when mouse is released and pause the video
        private void SkipForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _playerVM.VM_Speed /= 4f;
            if (_playerVM.VM_IsPlaying)
                _playerVM.stop();
        }

        // multiply the speed by 4 and reverse when the skip-backward button is held
        private void SkipBackward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // set to reverse before start()
            _playerVM.VM_IsReverse = true;
            if (!_playerVM.VM_IsPlaying)
            {
                _playerVM.start();
            }
            _playerVM.VM_Speed *= 4f;
        }
        // set speed back to normal and stop playing in reverse when mouse is released
        private void SkipBackward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _playerVM.VM_Speed /= 4f;
            _playerVM.VM_IsReverse = false;
            _playerVM.stop();
        }

        // set time to 0 when the stop (reset) button is clicked
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _playerVM.VM_Timestep = 0;
            // update FlightGear
            _playerVM.update();
            _playerVM.stop();
        }

        // event for when the slider is being dragged
        private void SendData_DragStarted(object sender, DragStartedEventArgs e)
        {
            // update FlightGear while the slider is held
            if (!_playerVM.VM_IsPlaying)
            {
                _playerVM.start();
                wasPaused = true;
            }
            else
                wasPaused = false;
        }

        // event for when dragging the slider is completed
        private void SendData_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            // stop the player if it was paused before dragging slider
            if (wasPaused)
                _playerVM.stop();
            
        }

        // event for when the value of the slider is changed
        private void Completed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // stop player if reached the last timestep
            if (TimestepSlider.Value == _playerVM.VM_Length && _playerVM.VM_IsPlaying)
            {
                _playerVM.stop();
            }
        }
    }
}
