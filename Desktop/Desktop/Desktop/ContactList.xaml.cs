using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ContactList.xaml
    /// </summary>
    public partial class ContactList : Page
    {
        public enum ChatType { DM = 1, GroupChat }
        Firebase fire;
        Person me = new Person();
        List<Person> friends = new List<Person>();
        List<Person> allUsers = new List<Person>();
        List<GroupInfo> groupInfo = new List<GroupInfo>();
        MainScreen mainscreen;
        int chatType;
        int startIndexOfGroups = 0;
        int startIndexOfPeople = 0;

        public ContactList(MainScreen ms, int chatT)
        {
            InitializeComponent();
            mainscreen = ms;
            fire = new Firebase(ms);
            ChangeDimensions(mainscreen.frameDMList.Height - 10, mainscreen.frameDMList.Width - 10);    //fit to frameDMList
            chatType = chatT;

            try
            {
                Get_Details();     //Get details of all users, from the database
            }
            catch { }
        }

        private void ChangeDimensions(double height, double width)
        {
            Height = height;
            Width = width;
            ListBox1.Height = Height;
            ListBox1.Width = Width;
        }

        public MainScreen Get_Main_Screen()
        {
            return mainscreen;
        }

        public Firebase Get_Firebase()
        {
            return fire;
        }

        public Person Get_Myself()
        {
            return me;
        }

        //Get details of either logged in person or all users
        public async void Get_Details()
        {
            me = await fire.GetPerson(Properties.Settings.Default.SignedInID);

            if (chatType == 1)
            {
                friends = await fire.GetAllFriends(me.uid);

                AddPerson("", "", "btnFriends", true);      //Add line with Add Friends button
                AddPerson("", "", "btnRequests", true);      //Add line with Add Friends button

                if (friends != null)
                {
                    for (int x = 0; x < friends.Count; x++)
                    {
                        AddPerson(friends[x].photoURL, friends[x].displayName, friends[x].uid, false);
                    }
                }
                else
                {
                    if (MessageBox.Show("You do not currently have any friends. Lets go find them.", "Find Friends", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        DisplayAllUsers(false);
                    }
                }
            }
            else
            {
                groupInfo = await fire.GetUsersGroups(me.uid);
               
                AddPerson("", "", "btnAddGroups", true);      //Add line with Add Friends button
                AddPerson("", "", "btnCreateGroups", true);      //Add line with Add Friends button

                if (groupInfo != null)
                {
                    for (int x = 0; x < groupInfo.Count; x++)
                    {
                        var thisGroupInfo = await fire.GetUsersGroupInfo(groupInfo[x].owner, groupInfo[x].groupName);
                        AddPerson(thisGroupInfo.groupimage, groupInfo[x].groupName, "", false);
                    }
                }                
            }       
        }

        public async void DisplayAllUsers(bool hasFriends)
        {
            ChangeDimensions(mainscreen.frame.Height - 10, mainscreen.frame.Width - 10);        //fit to frame

            mainscreen.frame.Navigate(this);        //Show this page

            mainscreen.Get_TrayBalloon().Show_Notification("Finding All Users", "Retrieving all users. Please be patient.");

            allUsers = await fire.GetAllUsers();        //get all users from Firebase
            allUsers.Remove(me);

            try
            {
                if (hasFriends)
                {
                    friends = await fire.GetAllFriends(me.uid);     //get all friends from Firebase
                }
            }
            catch
            {

            }

            for (int x = 0; x < allUsers.Count; x++)        //for all users
            {
                bool isFriend = false;

                if (hasFriends)
                {
                    for (int y = 0; y < friends.Count; y++)
                    {
                        if (allUsers[x].uid == friends[y].uid)
                        {
                            isFriend = true;
                            y = friends.Count;
                        }
                    }
                }

                try
                {
                    if (isFriend)
                    {
                        AddPerson(allUsers[x].photoURL, allUsers[x].displayName, allUsers[x].uid, false);       //add to the list
                    }
                    else
                    {
                        AddPerson(allUsers[x].photoURL, allUsers[x].displayName, allUsers[x].uid, true);       //add to the list
                    }
                }
                catch
                {

                }
            }
        }

        public async Task<List<Person>> GetFriends()
        {
            friends = await fire.GetAllFriends(me.uid);     //get all friends from Firebase
            return friends;
        }

        //Add person to the page
        public void AddPerson(string photo, string displayname, string uid, bool needsAddButton)
        {
            List<UIElement> childElements = new List<UIElement>();
            string buttonText = "";
            int buttonHeight = 30;
            int buttonWidth = 30;
            int fontSize = 14;

            if (photo == "" || photo == null)     //if no profile picture is found
            {
                photo = "pack://application:,,,/Resources/bread.gif";
            }

            Button img = new Button     //Profile Picture
            {
                IsHitTestVisible = false,
                Background = new ImageBrush(GetImage(new Uri(photo))),
                Height = buttonHeight,
                Width = buttonWidth
            };

            childElements.Add(img);

            Label nameLabel = new Label     //Name
            {
                Content = displayname,
                FontFamily = new FontFamily("Lucida Sans"),
                FontSize = fontSize
            };

            childElements.Add(nameLabel);

            if (needsAddButton)
            {
                if (uid == "btnFriends")    //if button is for friends
                {
                    buttonText = "ADD FRIEND";
                    startIndexOfPeople++;
                }
                else if (uid == "btnRequests")    //if button is for requests
                {
                    buttonText = "VIEW FRIEND REQUESTS";
                    startIndexOfPeople++;
                }
                else if (uid == "btnAddGroups")    //if button is for groups
                {
                    buttonText = "ADD MEMBER";
                    startIndexOfGroups++;
                }
                else if (uid == "btnCreateGroups")    //if button is for groups
                {
                    buttonText = "CREATE GROUP";
                    startIndexOfGroups++;
                }
                else    //if anything else happens, just in case
                {
                    buttonText = "ADD";
                }

                TextBlock addFriendText = new TextBlock
                {
                    Text = buttonText,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Normal,
                    FontSize = fontSize
                };

                Button addFriendButton = new Button
                {
                    IsHitTestVisible = true,
                    Content = addFriendText,
                    Background = Brushes.Chocolate,
                    BorderBrush = Brushes.Transparent,
                    Height = buttonHeight,
                    Name = uid,
                    Padding = new Thickness(2)
                };

                if (uid == "btnAddGroups")
                {
                    addFriendButton.Click += AddGroupButton_Click;     //Set button's click event
                }
                else if (uid == "btnCreateGroups")
                {
                    addFriendButton.Click += CreateGroupButton_Click;     //Set button's click event
                }
                else if (uid == "btnRequests")
                {
                    addFriendButton.Click += ViewFriendRequests_Click;     //Set button's click event
                }
                else
                {
                    addFriendButton.Click += AddFriendButton_Click;     //Set button's click event
                }

                childElements.Add(addFriendButton);
            }

            StackPanel panel = new StackPanel       //Contact panel, for each contact
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,

                Width = this.Width - 50,
            };

            for (int i = 0; i < childElements.Count; i++)
            {
                panel.Children.Add(childElements[i]);
            }

            ListBox1.Items.Add(panel);      //Add contact panel to the listbox on the page
        }

        //Added click event for the add group button
        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            //Get all groups
            //Display all groups
        }

        //Added click event for the create group button
        private void CreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            AddGroupWindow groupWindow = new AddGroupWindow(this, mainscreen, fire);

            groupWindow.Show();
        }

        //Added click event for the friend requests button
        private async void ViewFriendRequests_Click(object sender, RoutedEventArgs e)
        {
            Button button = new Button();
            button = (Button)sender;

            List<Person> friends = new List<Person>();

            friends = await fire.GetFriendRequests(me.uid);

            mainscreen.Get_TrayBalloon().Show_Friend_Requests(friends);
        }

        //Added click event for the add friend button
        private async void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = new Button();
            button = (Button)sender;

            if (button.Name == "btnFriends")        //if button is the add friends button (currently named btnFriends through code)
            {
                ListBox1.Items.Clear();     //clear the list

                if (friends == null)    //then if user has no friends
                {
                    DisplayAllUsers(false);     //Display all users, saying hasFriends is false
                }
                else
                {
                    DisplayAllUsers(true);      //Display all users, saying hasFriends is true
                }
            }
            else
            {
                //Place code here to add friends
                if (MessageBox.Show("Are you sure you want to add this friend?", "Add friend", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    friends = await fire.GetAllFriends(Properties.Settings.Default.SignedInID);     //Update friends list

                    Person futureFriend = new Person();      //Create my future friend

                    futureFriend = await fire.GetPerson(button.Name);

                    bool isAlreadyMyFriend = false;     //Is future friend already my friend?

                    int insertAtIndex = 0;      //create index where to insert new friend

                    if (friends != null)  //if friends are found
                    {
                        for (int i = 0; i < friends.Count; i++)     //for all my friends
                        {
                            if (friends[i].uid == futureFriend.uid)     //if future friend is already one of my friends
                            {
                                isAlreadyMyFriend = true;
                                i = friends.Count;      //exit for loop (scared to use break...)
                            }
                        }

                        insertAtIndex = friends.Count;  //assign next open position to insertAtIndex
                    }

                    if (!isAlreadyMyFriend)     //if future friend is not already my friend
                    {
                        fire.Insert($"friends/{me.uid}/{insertAtIndex}", futureFriend);     //Insert into my list of friends

                        List<Person> futureFriendsFriends = new List<Person>();     //Create friends of my future friend

                        futureFriendsFriends = await fire.GetAllFriends(futureFriend.uid);      //Get list of all friends of the future friend

                        insertAtIndex = 0;  //reset index

                        if (futureFriendsFriends != null)  //if friends of future friend is found
                        {
                            insertAtIndex = futureFriendsFriends.Count;  //assign next open position to insertAtIndex
                        }

                        Person iAmTheFriend = new Person();       //Create me as the friend to my future friend
                        iAmTheFriend = await fire.GetPerson(me.uid);

                        fire.Insert($"friends/{futureFriend.uid}/{insertAtIndex}", iAmTheFriend);     //Insert into friend's list of friends

                        mainscreen.Get_TrayBalloon().Show_Notification("Add Friends", "Friend Added");

                        friends = await fire.GetAllFriends(Properties.Settings.Default.SignedInID);     //Update friends list




                        //Spam
                        ContactList clDM = new ContactList(mainscreen, 1);
                        mainscreen.frameDMList.Navigate(clDM);
                    }
                    else
                    {
                        mainscreen.Get_TrayBalloon().Show_Notification("Add Friends", "User is already your friend");
                    }
                }
            }
        }

        //Get an image with a given uri and return as BitmapImage
        public BitmapImage GetImage(Uri uri)
        {
            BitmapImage img = new BitmapImage();

            try
            {
                img.BeginInit();
                img.UriSource = uri;
                img.EndInit();

                return img;
            }
            catch (Exception e)
            {
                //mainscreen.Get_TrayBalloon().Show_Notification("Get Image Error", e.Message);
                return null;
            }
        }

        public async void OpenDM(object sender, MouseEventArgs e)
        {
            try
            {
                try
                {
                    DirectMessage dm = new DirectMessage(groupInfo[ListBox1.SelectedIndex - startIndexOfGroups], (int)ChatType.GroupChat, mainscreen, this);
                    mainscreen.frame.Navigate(dm);
                }
                catch
                {
                    string contact = friends[ListBox1.SelectedIndex - startIndexOfPeople].uid;

                    DirectMessage dm = new DirectMessage(contact, (int)ChatType.DM, mainscreen, this);
                    mainscreen.frame.Navigate(dm);
                }
            }
            catch { }       
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
