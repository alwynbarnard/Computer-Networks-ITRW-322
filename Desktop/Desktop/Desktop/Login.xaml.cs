using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
using Xceed.Wpf.Toolkit;
namespace Desktop
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public string FirebaseAppKey = Properties.Settings.Default.AuthFirebaseKey;
        public string FirebaseAppUri = Properties.Settings.Default.FirebaseAppUri;
        public MainScreen mainScreen;
        //Check for internet connection
        
        private bool CheckConnection(String URL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public Login(MainScreen ms)
        {
            InitializeComponent();
            mainScreen = ms;
        }
        public async Task signinAsync()
        {
            if (CheckConnection("https://stackoverflow.com/questions/5405895/how-to-check-the-internet-connection-with-net-c-and-wpf") == false)
            {
                bool found = false;

                using (Stream str = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "//Users.txt", FileMode.Open, FileAccess.Read))
                {
                    string ID = "";
                    string display = "";
                    using (StreamReader reader = new StreamReader(str))
                    {
                        while (!reader.EndOfStream && found ==false)
                        {
                            string line = reader.ReadLine();
                            string username = line.Substring(0, line.IndexOf(","));
                            line = line.Remove(0, line.IndexOf(",") + 1);
                            if (username.ToUpper()== txtName.Text.ToUpper())
                            {
                                found = true;
                                ID = line.Substring(line.LastIndexOf(",") + 1);
                                line = line.Remove(line.LastIndexOf(","));
                                display = line.Substring(line.LastIndexOf(",") + 1);
                            } 
                           
                        }
                    }
                    if (found)
                    {
                       
                        Properties.Settings.Default.SignedInID = ID;
                        Properties.Settings.Default.DisplayName = display;
                        Properties.Settings.Default.Save();

                        ContactList clDM = new ContactList(mainScreen, 1);
                        mainScreen.frameDMList.Navigate(clDM);

                        ContactList clGr = new ContactList(mainScreen, 2);
                        mainScreen.frameGroupChats.Navigate(clGr);
                    }
                }
            }
            else
            {
                try
                {
                    loadingbar.IsBusy = true;
                    // Convert the access token to firebase token
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAppKey));

                    var data = await auth.SignInWithEmailAndPasswordAsync(txtName.Text, txtPassword.Password);

                    // Setup FirebaseClient to use the firebase token for data requests
                    var db = new FirebaseClient(
                           FirebaseAppUri,
                           new FirebaseOptions
                           {
                               AuthTokenAsyncFactory = () => Task.FromResult(data.FirebaseToken)
                           });

                    // TODO: your path within your DB structure.
                    var dbData = await db
                        .Child("users")
                        .Child(data.User.LocalId)
                        .OnceAsync<object>(); // TODO: custom class to represent your data instead of just object

                    // TODO: present your data
                    Properties.Settings.Default.SignedInID = data.User.LocalId;
                    if (cbxRemember.IsChecked == true)
                    {
                        Properties.Settings.Default.RememberMe = true;
                        Properties.Settings.Default.Email = txtName.Text;
                        Properties.Settings.Default.Password = txtPassword.Password;

                    }
                    else
                    {
                        Properties.Settings.Default.RememberMe = false;
                        Properties.Settings.Default.Email = "";
                        Properties.Settings.Default.Password = "";
                    }
                    Properties.Settings.Default.Save();
                    loadingbar.IsBusy = false;
                    ContactList clDM = new ContactList(mainScreen, 1);
                    mainScreen.frameDMList.Navigate(clDM);

                    ContactList clGr = new ContactList(mainScreen, 2);
                    mainScreen.frameGroupChats.Navigate(clGr);
                }
                catch (Exception ex)
                {
                    mainScreen.Get_TrayBalloon().Show_Notification("Invalid credentials. Please try again", ex.Message);
                }

                CoverPage coverPage = new CoverPage(mainScreen);
                mainScreen.Get_Frame().Navigate(coverPage);

                try
                {
                    List<CommandObject> commands = new List<CommandObject>();
                    commands = await mainScreen.Get_TrayBalloon().Get_Firebase().GetAllCommands(Properties.Settings.Default.SignedInID);

                    mainScreen.Get_TrayBalloon().Show_Menu(commands);

                }
                catch
                {
                    //mainScreen.Get_TrayBalloon().Show_Notification("Commands", "No commands found");
                    loadingbar.IsBusy = false;
                }
            }    
        }
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadString("https://www.latlmes.com/arts/return-of-the-golden-age-of-comics-1");
                await signinAsync();

                mainScreen.frame.Width = 582;
                mainScreen.frame.Height = 366;
                mainScreen.frame.Margin = new Thickness(302, 23, 0, 0);
            }
            catch
            { 
                btnLogin.IsEnabled = false;
                btnProxy.IsEnabled = false;
                btnSignUp.IsEnabled = false;
            }
        }

        private void lblForgotPassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Inputbox in3 = new Inputbox(mainScreen);
            in3.ShowDialog();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUpPage = new SignUp(mainScreen);
            mainScreen.frame.Navigate(signUpPage);
        }
        private void BtnProxy_Click(object sender, RoutedEventArgs e)
        {
            Proxy signUpPage = new Proxy(mainScreen);
            mainScreen.frame.Navigate(signUpPage);
        }

        private void BtnLogin_Loaded(object sender, RoutedEventArgs e)
        {
            if (CheckConnection("https://stackoverflow.com/questions/5405895/how-to-check-the-internet-connection-with-net-c-and-wpf") == false)
            {

                mainScreen.Get_TrayBalloon().Show_Notification("Offline Login", "No internet connection, please enter only your email for offline login");

                btnSignUp.IsEnabled = false;
                lblForgot.IsEnabled = false;
                txtPassword.IsEnabled = false;
                btnLogin.IsEnabled = false;
                btnProxy.IsEnabled = false;
            }
           
            if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "//Users.txt"))
            {
                File.Create(System.AppDomain.CurrentDomain.BaseDirectory + "//Users.txt");
            }
            if (Properties.Settings.Default.RememberMe==true)
            {
                txtName.Text = Properties.Settings.Default.Email;
                txtPassword.Password = Properties.Settings.Default.Password;
                cbxRemember.IsChecked = true; 
            }
   
        }
    }
}
