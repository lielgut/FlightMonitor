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

        private void FForward_Click(object sender, RoutedEventArgs e)
        {
            if (playerVM.VM_Timestep + 5 < playerVM.VM_Length)
                playerVM.VM_Timestep += 5;
            else
                playerVM.VM_Timestep = playerVM.VM_Length;
        }

        private void FRewind_Click(object sender, RoutedEventArgs e)
        {
            playerVM.VM_IsReverse = true;
            playerVM.VM_Timestep -= 5;
        }

        private void FRewind_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_IsReverse = true;
            playerVM.VM_Speed *= 2;
        }

        private void FForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_Speed /= 2;
        }

        private void FForward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_Speed *= 2;
        }

        private void FRewind_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            playerVM.VM_Speed /= 2;
        }
    }
}
