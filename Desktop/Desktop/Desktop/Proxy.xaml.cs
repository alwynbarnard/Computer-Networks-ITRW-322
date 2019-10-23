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
using System.Text.RegularExpressions;
using System.Net;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for Proxy.xaml
    /// </summary>
    public partial class Proxy : Page
    {
        private MainScreen mainScreen;
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        public Proxy(MainScreen ms)
        {
            InitializeComponent();
            mainScreen = ms;
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            Login loginPage = new Login(mainScreen);
            mainScreen.frame.Navigate(loginPage);
        }
        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            Login loginPage = new Login(mainScreen);
            mainScreen.frame.Navigate(loginPage);
        }

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPort.Text) || string.IsNullOrEmpty(txtWebPage.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("All fields are required. Please fill them in before proceeding");
            }
            else
            {
                ICredentials cred;
                cred = new NetworkCredential(txtUsername.Text, txtPassword.Password);

                WebRequest.DefaultWebProxy = new WebProxy(txtWebPage.Text + ':' + txtPort.Text, true,null,cred);
                /*
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://console.firebase.google.com/u/1/project/itrw322final-464ab/database/itrw322final-464ab/data");
                WebProxy myproxy = new WebProxy(txtWebPage.Text+ ':' + txtPort.Text , true);
                myproxy.BypassProxyOnLocal = false;
                request.Proxy = myproxy;
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                MessageBox.Show(response.ToString());*/
            }
        }
    }
}
