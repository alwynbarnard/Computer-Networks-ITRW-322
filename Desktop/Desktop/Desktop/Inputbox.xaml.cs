using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for Inputbox.xaml
    /// 
    /// </summary>
    public partial class Inputbox : Window
    {
        public string FirebaseAppKey = Properties.Settings.Default.AuthFirebaseKey; // https://console.firebase.google.com/
        public string FirebaseAppUri = Properties.Settings.Default.FirebaseAppUri;

        MainScreen mainScreen;

        public Inputbox(MainScreen ms)
        {
            InitializeComponent();
            mainScreen = ms;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text !="" && Regex.IsMatch(txtEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAppKey));
                auth.SendPasswordResetEmailAsync(txtEmail.Text);
                this.Close();
            }
            else
            {
                mainScreen.Get_TrayBalloon().Show_Notification("Invalid Email", "An invalide email address was entered, please try again");
            }
        }
    }
}
