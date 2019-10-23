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
using System.Windows.Shapes;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for AddGroupWindow.xaml
    /// </summary>
    public partial class AddGroupWindow : Window
    {
        private ContactList contactList;
        private Firebase fireBase;
        private MainScreen mainScreen;
        private string filepath = "";   //picture file path

        public AddGroupWindow()
        {
            //Do Nothing
        }

        public AddGroupWindow(ContactList cl, MainScreen ms, Firebase fb)
        {
            InitializeComponent();
            contactList = cl;
            mainScreen = ms;
            fireBase = fb;
        }

        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<GroupInfo> groupsnames = new List<GroupInfo>();
                groupsnames = await fireBase.GetUsersGroups(contactList.Get_Myself().uid);       //groups, name and owner

                GroupInfo newGroup = new GroupInfo();

                string downloadurl = await fireBase.SendFile(filepath, "ProfilePics");

                newGroup.groupName = txtDisplayName.Text;
                newGroup.owner = Properties.Settings.Default.SignedInID;
                newGroup.groupimage = downloadurl;
                newGroup.msgboard = new List<Message>();
                newGroup.members = new List<Person>() { contactList.Get_Myself() };

                fireBase.InsertGroup(newGroup, Properties.Settings.Default.SignedInID);

                MessageBox.Show("Group successfully created!");

                ContactList clGM = new ContactList(mainScreen, 2);
                mainScreen.frameGroupChats.Navigate(clGM);
                Close();
            }
            catch
            {
                MessageBox.Show("Error. Please try enter all of the fields correctly.");
            }
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
                        GroupPic.Source = bitmap;
                        GroupPic.Height = 93;
                        GroupPic.Width = 124;
                    }
                    else
                    {
                        MessageBox.Show("This file type is not supported.", "Invalid file",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void GroupPic_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
