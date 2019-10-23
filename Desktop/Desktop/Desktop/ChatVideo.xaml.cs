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

namespace Desktop
{
    /// <summary>
    /// Interaction logic for ChatVideo.xaml
    /// </summary>
    public partial class ChatVideo : UserControl
    {
        int count;
        public ChatVideo()
        {
            InitializeComponent();
        }

        public ChatVideo(string displayname, string path, string datetime, int role, SolidColorBrush color)
        {
            InitializeComponent();

            InitializeComponent();
            lblDisplayName.Content = displayname;

            mediaElement.Source = new Uri(path, UriKind.Absolute);
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Stop;
            mediaElement.Visibility = Visibility.Visible;
            image.MinWidth = mediaElement.MinWidth = mediaElement.MaxWidth = 300;
            image.MinHeight = mediaElement.MinHeight = mediaElement.MaxHeight = 257;

            lblDateTime.Content = datetime;

            if (role == 1)
                grid.Background = Brushes.Khaki;
            else
                grid.Background = Brushes.Azure;

            lblDisplayName.Foreground = color;
            image.Source = new BitmapImage(new Uri("Resources/Play.png", UriKind.Relative));
        }

        private void MediaElement1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            count++;

            if (count % 2 == 1)
            {
                image.Visibility = Visibility.Hidden;
                mediaElement.Play();
            }  
            else
                mediaElement.Pause();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(0);
            mediaElement.Pause();
            count++;

            image.Source = new BitmapImage(new Uri("Resources/Replay.png", UriKind.Relative));
            image.Visibility = Visibility.Visible;
        }
    }
}
