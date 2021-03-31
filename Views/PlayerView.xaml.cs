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
    }
}
