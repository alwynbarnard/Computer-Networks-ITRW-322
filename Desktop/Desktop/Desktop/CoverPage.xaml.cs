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
    /// Interaction logic for CoverPage.xaml
    /// </summary>
    public partial class CoverPage : Page
    {
        MainScreen mainScreen;

        public CoverPage(MainScreen ms)
        {
            InitializeComponent();
            mainScreen = ms;
            Height = mainScreen.Get_Frame().Height - 10;
            Width = mainScreen.Get_Frame().Width - 10;
            CoverImage.Height = Height;
            CoverImage.Width = Width;
            CoverImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/bread.gif"));
            CoverImage.Stretch = Stretch.Fill;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
           
        }
    }
}
