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
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : Page
    {
        ChromiumWebBrowser web;

        public Browser()
        {
            InitializeComponent();

            web = new ChromiumWebBrowser();
            web.Address = "https://www.google.com";
            Grid.SetRow(web, 0);
            Grid1.Children.Add(web);
        }
    }
}
