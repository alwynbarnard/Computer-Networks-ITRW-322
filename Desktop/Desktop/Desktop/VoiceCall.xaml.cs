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
    /// Interaction logic for VoiceCall.xaml
    /// </summary>
    public partial class VoiceCall : Page
    {
        MainScreen mainScreen;
        public VoiceCall(MainScreen ms)
        {
            InitializeComponent();
            mainScreen = ms;
        }

        private void BtnEndCallVideo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
