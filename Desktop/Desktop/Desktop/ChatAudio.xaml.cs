using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Threading;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for ChatAudio.xaml
    /// </summary>
    public partial class ChatAudio : UserControl
    {
        int count = 0;
        public ChatAudio()
        {
            InitializeComponent();
        }

        public ChatAudio(string displayname, string path, string datetime, int role, SolidColorBrush color)
        {
            InitializeComponent();
            lblDisplayName.Content = displayname;
            mediaElement.Source = new Uri(@path, UriKind.Absolute);
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Stop;
            lblDateTime.Content = datetime;

            btnPlayPause.MinWidth = 52;
            btnPlayPause.MinHeight = 22;

            try
            {
                lblName.Content = path.Substring(path.LastIndexOf("\\") + 1).Replace('_', ' ');
            }
            catch
            {
                lblName.Content = path.Substring(path.LastIndexOf("/") + 1).Replace('_', ' ');
            }
            
            if (role == 1)
                grid.Background = Brushes.Khaki;
            else
                grid.Background = Brushes.Azure;

            lblDisplayName.Foreground = color;
        }

        /*
        public void Tick()
        {
            string displayTime;
            double pos = mediaElement.Position.TotalSeconds;
            slTime.Value = pos;

            if (pos < 10)
                displayTime = $"00:0{pos}";
            else if (pos < 60)
                displayTime = $"00:{pos}";
            else if (pos >= 60 && pos < 3600)
                displayTime = $"{(pos % 3600) - (pos % 60)}:{pos % 60}";
            else
            {

            }
        }
        */

        private void BtnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            count++;

            if (count % 2 == 1)
            {
                mediaElement.Play();
                btnPlayPause.Content = "Pause";
            }
            else
            {
                mediaElement.Pause();
                btnPlayPause.Content = "Play";
            } 
        }
    }
}
