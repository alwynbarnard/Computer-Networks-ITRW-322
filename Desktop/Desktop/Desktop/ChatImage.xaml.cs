using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ChatImage.xaml
    /// </summary>
    public partial class ChatImage : UserControl
    {
        public ChatImage()
        {
            InitializeComponent();
        }

        private string PathName { get; set; }

        public ChatImage(string name, string path, string datetime, int role, SolidColorBrush color)
        {
            InitializeComponent();

            lblDisplayName.Content = name;
            PathName = path;
            imageContainer.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            lblDateTime.Content = datetime;

            if (role == 1)
                grid.Background = Brushes.Khaki;
            else
                grid.Background = Brushes.Azure;

            lblDisplayName.Foreground = color;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(PathName);
            }
            catch { }
        }
    }
}
