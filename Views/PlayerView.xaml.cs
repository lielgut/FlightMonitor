using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ex1.ViewModels;

namespace ex1.Views
{
    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private PlayerViewModel playerVM;
        //readonly TimeSpan interval = TimeSpan.FromSeconds(0.5);
        //DateTime mouseDown = DateTime.Now;
        internal PlayerViewModel PlayerVM
        {
            get
            {
                return playerVM;
            }
            set
            {
                playerVM = value;
            }
        }
        public PlayerView()
        {
            InitializeComponent();
            DataContext = playerVM;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (playerVM.VM_IsPlaying)
            {
                playerVM.fc.stop();
                playPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            }
            else
            {
                playerVM.fc.start();
                playPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            }
        }
        private void FForward5_Click(object sender, RoutedEventArgs e)
        {
            // jump forward by 50 timesteps (5 seconds)
            if (playerVM.VM_Timestep + 50 <= playerVM.VM_Length)
                playerVM.VM_Timestep += 50;
            else
                playerVM.VM_Timestep = playerVM.VM_Length;
            playerVM.fc.SendCurrentData();
        }

        private void FRewind5_Click(object sender, RoutedEventArgs e)
        {

            if (playerVM.VM_Timestep - 50 >= 0)
                playerVM.VM_Timestep -= 50;
            else
                playerVM.VM_Timestep = 0;
            playerVM.fc.SendCurrentData();
        }



        private void FForward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                playerVM.VM_Speed *= 2f;
            
        }
        private void FForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_Speed /= 2f;
        }
        private void FRewind_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_IsReverse = true;
            playerVM.VM_Speed *= 2f;
        }
        private void FRewind_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_IsReverse = false;
            playerVM.VM_Speed /= 2f;
        }

        private void SkipForward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!playerVM.VM_IsPlaying)
            {
                playerVM.fc.start();
                playPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            }
            playerVM.VM_Speed *= 4f;
        }

        private void SkipForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_Speed /= 4f;
            playerVM.fc.stop();
            playPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
        }

        private void SkipBackward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!playerVM.VM_IsPlaying)
            {
                playerVM.fc.start();
                playPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            }
            playerVM.VM_IsReverse = true;
            playerVM.VM_Speed *= 4f;
        }

        private void SkipBackward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_Speed /= 4f;
            playerVM.VM_IsReverse = false;
            playerVM.fc.stop();
            playPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            playerVM.VM_Timestep = 0;
            playerVM.fc.SendCurrentData();
            playerVM.fc.stop();
            playPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
        }

        private void SendData_DragLeave(object sender, DragEventArgs e)
        {
            if (!playerVM.VM_IsPlaying)
                playerVM.fc.SendCurrentData();
        }
    }
}
