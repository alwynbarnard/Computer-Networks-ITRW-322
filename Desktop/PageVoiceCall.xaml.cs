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
    /// Interaction logic for PageVoiceCall.xaml
    /// </summary>
    public partial class PageVoiceCall : Page
    {
        public PageVoiceCall()
        {
            InitializeComponent();
        }

        private void BtnEndCall_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEndCall_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Call ended");
        }
    }
}
