using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        private Firebase fire;
        private MainScreen mainScreen;
        public string AuthFirebaseAppKey = Properties.Settings.Default.AuthFirebaseKey;
        public string FirebaseAppUri = Properties.Settings.Default.FirebaseAppUri;
        public string filepath = "";

        public SignUp(MainScreen ms)
        {
            InitializeComponent();
            mainScreen = ms;
            fire = new Firebase(ms);
        }

        private async void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {

            if (txtName.Text == "" || txtPassword.Password == "" || txtPasswordConfirm.Password == "")
            {
                mainScreen.Get_TrayBalloon().Show_Notification("Missing fields", "One or more fields are not filled in");
            }
            else if (!Regex.IsMatch(txtName.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                mainScreen.Get_TrayBalloon().Show_Notification("Invalid email", "Please try again");
            }
            else 
            {

                loadingbar.IsBusy = true;
                if (txtPassword.Password.Length <= 6)
                {
                    mainScreen.Get_TrayBalloon().Show_Notification("Password too short", "Password must be more than 6 characters long!");

                    loadingbar.IsBusy = false;
                }
                else if (txtPassword.Password == txtPasswordConfirm.Password)
                {
                    Person signee = new Person(txtName.Text, txtPassword.Password, txtName.Text);


                    await signUpAsync();
                    mainScreen.Get_TrayBalloon().Show_Notification("Registration complete", "You have been registered!");

                    loadingbar.IsBusy = false;

                    Login loginPage = new Login(mainScreen);

                    mainScreen.frame.Navigate(loginPage);
                }
                else
                {
                    mainScreen.Get_TrayBalloon().Show_Notification("Invalid input", "Passwords don't match");
                    loadingbar.IsBusy = false;
                }
            }  
        }
        public async Task signUpAsync()
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(AuthFirebaseAppKey));
            var data = await auth.CreateUserWithEmailAndPasswordAsync(txtName.Text, txtPassword.Password, txtDisplayName.Text, false) ;

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
                .OnceAsync<object>();

            string downloadurl = await fire.SendFile(filepath, "ProfilePics");
            Person sign = new Person(txtDisplayName.Text, downloadurl, data.User.LocalId);
            fire.Insert("users/" + data.User.LocalId, sign);
            //Write to file for local use. 
            using (Stream str = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "//Users.txt", FileMode.Append, FileAccess.Write))
            {
                // Declare a StreamWriter object that can be used to write text data to the file
                using (StreamWriter writer = new StreamWriter(str))
                {
                    // Write a line of text to the file
                    writer.WriteLine(txtName.Text+","+txtDisplayName.Text + "," + data.User.LocalId);
                }
            }

        }
        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            Login loginPage = new Login(mainScreen);
            mainScreen.frame.Navigate(loginPage);
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            using (var file = new System.Windows.Forms.OpenFileDialog())
            {
                //Filter what can be seen
                file.Filter = "All Files (*.*)|*.*";

                //If a file is actually chosen
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Check if the extension is supported by the program
                    bool validExtension = false;
                    string extension = file.FileName.Substring(file.FileName.LastIndexOf("."));

                    string[] imageExtensions = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".bmap" };
                    if (imageExtensions.Contains(extension.ToLower()))
                    {
                        validExtension = true;
                    }
                    if (validExtension)
                    {
                        filepath = file.FileName;
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(filepath);
                        
                        bitmap.EndInit();
                        ProfilePic.Source = bitmap;
                        ProfilePic.Height = 93;
                        ProfilePic.Width = 124;
                    }
                    else
                    {
                        mainScreen.Get_TrayBalloon().Show_Notification("Invalid File", "This file type is not supported");
                    }
                }
            }
        }
    }
}
