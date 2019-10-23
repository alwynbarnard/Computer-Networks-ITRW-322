using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;
using Hardcodet.Wpf;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Input;

namespace Desktop
{
    public class TrayBalloon
    {
        Firebase firebase;
        ListBox listBoxCommands = new ListBox();
        ListBox listBoxFriends = new ListBox();
        ListBox listBoxRequests = new ListBox();
        MainScreen mainScreen;
        TaskbarIcon taskbarIcon;
        DirectMessage directMessage;
      
        public TrayBalloon()
        {

        }
        public TrayBalloon(MainScreen ms)
        {
            mainScreen = ms;
            firebase = new Firebase(ms);
        }
        public void Start_Tray_Balloon()
        {
            taskbarIcon = new TaskbarIcon();
            Icon icon;
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/bread.ico")).Stream;
            icon = new Icon(iconStream);

            taskbarIcon.Icon = icon;
            taskbarIcon.ToolTipText = "Bread Mail";

            //ShowMenu(taskbarIcon);
        }

        public void Show_Menu(List<CommandObject> commands)
        {
            int fontSize = 24;
            int buttonHeight = 50;
            int buttonWidth = 300;

            listBoxCommands.Items.Clear();
           
            //Heading of list
            Label heading = new Label();
            heading.Content = "List of Commands";
            heading.FontSize = fontSize;
            listBoxCommands.Items.Add(heading);

            for (int i = 0; i < commands.Count; i++)        //for each command, add to the listbox
            {
                Button listButton = new Button();
                listButton.Content = commands[i].Command;
                listButton.Click += TrayButton_Clicked;
                listButton.Height = buttonHeight;
                listButton.Width = buttonWidth;
                listButton.FontSize = fontSize;
                listBoxCommands.Items.Add(listButton);
            }

            Button closeButton = new Button();
            closeButton.Content = "Close this tray";
            closeButton.Height = buttonHeight;
            closeButton.Width = buttonWidth;
            closeButton.FontSize = fontSize;
            closeButton.Click += TrayButton_Clicked;
            listBoxCommands.Items.Add(closeButton);

            //taskbarIcon.TrayPopup = listBox;
            //taskbarIcon.TrayBalloonTipClicked += TrayPopOpen;

            taskbarIcon.ShowCustomBalloon(listBoxCommands, PopupAnimation.Scroll, null);
        }

        public void Show_Friend_Requests(List<Person> newFriends)
        {
            listBoxRequests.Items.Clear();

            int numberOfRequests = 0;

            if (newFriends != null)
            {
                numberOfRequests = newFriends.Count;
            }

            int fontSize = 24;
            int buttonHeight = 50;
            int buttonWidth = 300;

            //Heading of list
            Label heading = new Label();
            heading.Content = "List of Friend Requests";
            heading.FontSize = fontSize;
            listBoxRequests.Items.Add(heading);

            for (int i = 0; i < numberOfRequests; i++)        //for each friend, add to the listbox
            {
                List<UIElement> childElements = new List<UIElement>();

                Label nameLabel = new Label     //Name
                {
                    Content = newFriends[i].displayName,
                    FontFamily = new System.Windows.Media.FontFamily("Lucida Sans"),
                    FontSize = fontSize
                };

                childElements.Add(nameLabel);

                Button accept = new Button     //Accept button
                {
                    Name = newFriends[i].uid,
                    IsHitTestVisible = true,
                    Content = "Accept",
                    Height = buttonHeight,
                    Width = buttonHeight,

                };

                accept.Click += friendsList_Accept_Clicked;
                childElements.Add(accept);

                Button reject = new Button     //Reject button
                {
                    Name = newFriends[i].uid,
                    IsHitTestVisible = true,
                    Content = "Reject",
                    Height = buttonHeight,
                    Width = buttonHeight,
                };

                reject.Click += friendsList_Reject_Clicked;
                childElements.Add(reject);

                Button listButton = new Button();
                listButton.Content = newFriends[i].displayName;
                //listButton.Click += requestList_Clicked;
                listButton.Height = buttonHeight;
                listButton.Width = buttonWidth;
                listButton.FontSize = fontSize;

                StackPanel panel = new StackPanel       //Contact panel, for each contact
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                for (int j = 0; j < childElements.Count; j++)
                {
                    panel.Children.Add(childElements[j]);
                }

                listBoxRequests.Items.Add(panel);
            }

            Button closeButton = new Button();
            closeButton.Content = "Close this tray";
            closeButton.Height = buttonHeight;
            closeButton.Width = buttonWidth;
            closeButton.FontSize = fontSize;
            closeButton.Click += TrayButton_Clicked;
            listBoxRequests.Items.Add(closeButton);

            //taskbarIcon.TrayPopup = listBox;
            //taskbarIcon.TrayBalloonTipClicked += TrayPopOpen;

            taskbarIcon.ShowCustomBalloon(listBoxRequests, PopupAnimation.Scroll, null);
        }

        public void Show_Friends_Menu(List<Person> friends, DirectMessage dms)
        {
            directMessage = dms;
            listBoxFriends.Items.Clear();
            string photo = "";
            string name = "button";
            int fontSize = 24;
            int buttonHeight = 50;
            int buttonWidth = 300;
            int friendsCount = 0;
            //Heading of list
            Label heading = new Label();
            heading.Content = "List of Friends";
            heading.FontSize = fontSize;
            listBoxFriends.Items.Add(heading);

            if (friends != null)
            {
                friendsCount = friends.Count;
            }

            for (int i = 0; i < friendsCount; i++)
            {
                List<UIElement> childElements = new List<UIElement>();

                try
                {
                    photo = friends[i].photoURL;
                    name = friends[i].displayName;
                }
                catch
                {

                }

                if (photo == "" || photo == null)     //if no profile picture is found
                {
                    photo = "pack://application:,,,/Resources/bread.gif";
                }

                Button img = new Button     //Profile Picture
                {
                    IsHitTestVisible = false,
                    Background = new ImageBrush(new BitmapImage(new Uri(photo))),
                    Height = buttonHeight,
                    Width = buttonHeight
                };

                childElements.Add(img);

                Label nameLabel = new Label     //Name
                {
                    Content = name,
                    FontFamily = new System.Windows.Media.FontFamily("Lucida Sans"),
                    FontSize = fontSize
                };

                childElements.Add(nameLabel);

                StackPanel panel = new StackPanel       //Contact panel, for each contact
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                for (int j = 0; j < childElements.Count; j++)
                {
                    panel.Children.Add(childElements[j]);
                }

                Button listButton = new Button();
                listButton.Content = panel;
                listButton.Click += friendsList_Clicked;
                listButton.Name = friends[i].uid;
                listButton.Height = buttonHeight;
                listButton.Width = buttonWidth;
                listButton.FontSize = fontSize;
               
                listBoxFriends.Items.Add(listButton);        //Add friend to the panel
            }

            Button closeButton = new Button();
            closeButton.Content = "Close this tray";
            closeButton.Height = buttonHeight;
            closeButton.Width = buttonWidth;
            closeButton.FontSize = fontSize;
            closeButton.Click += TrayButton_Clicked;
            listBoxFriends.Items.Add(closeButton);

            taskbarIcon.ShowCustomBalloon(listBoxFriends, PopupAnimation.Scroll, null);
        }

        private async void friendsList_Accept_Clicked(object sender, EventArgs e)
        {
            Button thisButton = (Button)sender;
            Person me = new Person();
            Person personToAdd = new Person();
            FriendRequest request = new FriendRequest();
            List<Person> myFriends = new List<Person>();
            List<Person> personFriends = new List<Person>();

            string myUID = Properties.Settings.Default.SignedInID;
            int friendCount = 0;

            me = await firebase.GetPerson(myUID);
            personToAdd = await firebase.GetPerson(thisButton.Name);

            myFriends = await firebase.GetAllFriends(myUID);
            personFriends = await firebase.GetAllFriends(personToAdd.uid);

            request.sender = thisButton.Name;

            if (myFriends != null)
            {
                friendCount = myFriends.Count;
            }

            firebase.Insert($"friends/{myUID}/{friendCount}", personToAdd);

            if (personFriends != null)
            {
                friendCount = personFriends.Count;
            }

            firebase.Insert($"friends/{personToAdd.uid}/{friendCount}", me);

            firebase.DeleteFriendRequest(request);

            thisButton.IsEnabled = false;
            thisButton.Visibility = Visibility.Hidden;
            //listBoxRequests.Items.Remove(thisButton.Parent);
        }

        private void friendsList_Reject_Clicked(object sender, EventArgs e)
        {
            Button thisButton = (Button)sender;
            FriendRequest request = new FriendRequest();

            request.sender = thisButton.Name;
            firebase.DeleteFriendRequest(request);
        }

        private async void requestList_Clicked(object sender, EventArgs e)
        {
            Button thisButton = (Button)sender;
            Person personToAdd = new Person();

            personToAdd = await firebase.GetPerson(thisButton.Name);

        }

        private async void friendsList_Clicked(object sender, EventArgs e)
        {
            Button thisButton = (Button)sender;
            Person personToAdd = new Person();
            List<GroupInfo> personToAddGroups = new List<GroupInfo>();
            List<GroupInfo> myGroups = new List<GroupInfo>();
           
            string currentChatName = directMessage.Get_Current_Chat_Name();
            string friendUID = thisButton.Name;
            int myGroupsCount = 0;
            int theirGroupsCount = 0;

            personToAdd = await firebase.GetPerson(friendUID);
            
            try
            {
                GroupInfo myGroupInfo = new GroupInfo();
                string myID = Properties.Settings.Default.SignedInID;
                string myGroupName = "";
                int numberOfMembers = 0;
                bool isAlreadyThere = false;

                myGroups = await firebase.GetUsersGroups(myID);
                personToAddGroups = await firebase.GetUsersGroups(friendUID);

                if (myGroups != null)
                {
                    myGroupsCount = myGroups.Count;
                }

                if (personToAddGroups != null)
                {
                    theirGroupsCount = personToAddGroups.Count;
                }

                for (int i = 0; i < myGroupsCount; i++)
                {
                    if (currentChatName == myGroups[i].groupName)
                    {
                        if (myGroups[i].members != null)
                        {
                            numberOfMembers = myGroups[i].members.Count;
                        }

                        for (int j = 0; j < numberOfMembers; j++)
                        {
                            if (friendUID == myGroups[i].members[j].uid)
                            {
                                isAlreadyThere = true;
                            }
                        }

                        if (isAlreadyThere)
                        {
                            mainScreen.Get_TrayBalloon().Show_Notification("Adding To Group", personToAdd.displayName + " is already part of this group");
                        }
                        else
                        {
                            if (myGroups[i].members != null)
                            {
                                numberOfMembers = myGroups[i].members.Count;
                            }

                            myGroupName = myGroups[i].groupName;
                            myGroupInfo = await firebase.GetUsersGroupInfo(myID, myGroupName);
                           
                            firebase.Insert($"groups/{myID}/{myGroupName}/members/{numberOfMembers - 1}", personToAdd);
                            firebase.Insert($"groups/{personToAdd.uid}/{myGroupName}", myGroupInfo);
                            firebase.Insert($"groupsnames/{personToAdd.uid}/{theirGroupsCount}", myGroups[i]);

                            mainScreen.Get_TrayBalloon().Show_Notification("Adding To Group", personToAdd.displayName + " added successfully to the group " + currentChatName);
                        }

                        thisButton.IsEnabled = false;
                        thisButton.Visibility = Visibility.Hidden;
                        listBoxFriends.Items.Remove(thisButton);

                        i = myGroups.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                string textString = "Unable to add " + personToAdd.displayName + " to the current group";
                mainScreen.Get_TrayBalloon().Show_Notification("Adding To Group Error", textString);
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }

        private async void DeleteAllCommands()
        {
            int allCommandsCount = 0;

            string myUID = Properties.Settings.Default.SignedInID;
            List<CommandObject> allCommands = new List<CommandObject>();
            allCommands = await firebase.GetAllCommands(myUID);

            if (allCommands != null)
            {
                allCommandsCount = allCommands.Count;
            }

            for (int i = 0; i < allCommandsCount; i++)
            {
                firebase.DeleteCommand(allCommands[i]);
            }
        }

        public void TrayButton_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string content = clickedButton.Content.ToString();

            CommandObject thisCommand = new CommandObject();
            thisCommand.Command = content;

            if (content.Contains("ShowPopup"))
            {
                string message = content.Substring(content.IndexOf("=") + 1);

                Show_Notification("Commands", message);

                firebase.DeleteCommand(thisCommand);
                listBoxCommands.Items.Remove(clickedButton);
            }
            else if (content.Contains("ShowFileStructure"))
            {
                TreeViewForm treeForm = new TreeViewForm();
                DirectoryInfo directoryInfo = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
              
                treeForm.GetTreeView().Nodes.Add(TraverseDirectory(directoryInfo.FullName));
               
                treeForm.Show();

                firebase.DeleteCommand(thisCommand);
                listBoxCommands.Items.Remove(clickedButton);
            }
            else if (content.Contains("Openfile="))
            {
                string file = content.Substring(content.IndexOf("=") + 1);
                string path = System.Windows.Forms.Application.StartupPath + "\\" + file;

                if (File.Exists(path))
                {
                    Process.Start(path);
                }
                else
                {
                    mainScreen.Get_TrayBalloon().Show_Notification("Open File Error", "This file does not exist");
                }

                firebase.DeleteCommand(thisCommand);
                listBoxCommands.Items.Remove(clickedButton);
            }
            else if (content.Contains("ShutDown="))
            {
                int seconds = Convert.ToInt32(content.Substring(content.IndexOf("=") + 1));

                firebase.DeleteCommand(thisCommand);
                listBoxCommands.Items.Remove(clickedButton);

                Show_Notification("Commands", "This pc is going to shut down in " + seconds + " seconds");
                Process.Start("shutdown", "/s /t " + seconds);
            }
            else if (content.Contains("ShutDown"))
            {
                firebase.DeleteCommand(thisCommand);
                listBoxCommands.Items.Remove(clickedButton);

                Process.Start("shutdown", "/s /t 0");

            }
            else if (content.Contains("Sleep"))
            {
                firebase.DeleteCommand(thisCommand);
                listBoxCommands.Items.Remove(clickedButton);

                Show_Notification("Commands", "This pc is going to hibernate in a few seconds");
                System.Threading.Thread.Sleep(5000);

                System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, false, true);
            }
            else if (content.Contains("Close this tray"))
            {
                taskbarIcon.CloseBalloon();

                DeleteAllCommands();
            }
            else
            {
                Show_Notification("Commands", "Your C drive will be wiped and all USB devices rejected, unless you click here in the next 5 seconds...");
                System.Threading.Thread.Sleep(50);
            }
        }

        private System.Windows.Forms.TreeNode TraverseDirectory(string path)
        {
            System.Windows.Forms.TreeNode result = new System.Windows.Forms.TreeNode(path);
            foreach (var subdirectory in Directory.GetDirectories(path))
            {
                try
                {
                    result.Nodes.Add(TraverseDirectory(subdirectory));
                }
                catch
                {

                }
            }

            return result;
        }

        public void Show_Notification(string title, string text)
        {
            //show balloon with built-in icon
            //taskbarIcon.ShowBalloonTip(title, text, messageType);

            //show balloon with custom icon
            taskbarIcon.ShowBalloonTip(title, text, taskbarIcon.Icon, true);

            //hide balloon
            taskbarIcon.HideBalloonTip();
        }

        public Firebase Get_Firebase()
        {
            return firebase;
        }

    }
}
