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
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        bool lastSeen = false;
        bool profilePicture = false;
        bool accountStatus = false;
        bool accountAbout = false;
        public AccountPage()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbxLastSeen.IsChecked == true)
                lastSeen = true;
            if (cbxProfilePicture.IsChecked == true)
                profilePicture = true;
            if (cbxStatus.IsChecked == true)
                accountStatus = true;
            if (cbxAbout.IsChecked == true)
                accountAbout = true;
        }
        private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbxLastSeen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbxProfilePicture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbxAbout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbxStatus_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
