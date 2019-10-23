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
    /// Interaction logic for PageVideoCall.xaml
    /// </summary>
    public partial class PageVideoCall : Page
    {
        public PageVideoCall()
        {
            InitializeComponent();
        }

        private void BtnEndCallVideo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Call ended");
        }
    }
}
