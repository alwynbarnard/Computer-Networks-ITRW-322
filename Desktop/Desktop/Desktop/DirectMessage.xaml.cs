using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
    public partial class DirectMessage : Page
    {
        private MainScreen mainScreen;

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        //If the person is receiving a text, they are labeled a receiver (index value 1).
        //If they are sending a text, they are labeled a sender (index value 2).
        enum Role { Sender = 1, Receiver }
        enum FileType { Image = 1, Video, Audio }
        List<List<Object>> userColors = new List<List<object>>();
        SolidColorBrush[] presetUserColors = new SolidColorBrush[] { Brushes.Black, Brushes.Red, Brushes.Blue,
                                                                     Brushes.Green, Brushes.Orange, Brushes.Chocolate,
                                                                     Brushes.DeepPink, Brushes.SlateGray };

        //Other global variables
        Firebase firebaseDB;
        System.Windows.Forms.Timer timer;
        private ContactList contactList;

        int fileType,
            previousRole = (int)Role.Sender,
            encryptionOffset = 13,
            vnClickCount = 0,
            chatType;

        string selectedFilePath = null,
               chatID;

        Person me = new Person();
        GroupInfo groupInfo;

        //Lists in which Message objects are stored
        List<Message> myMessages = new List<Message>(),
                       newMessages = new List<Message>();

        List<Person> chatMembers = new List<Person>();

        string[] imageExtensions = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".bmap" };
        string[] videoExtensions = new string[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".gif" };
        string[] audioExtensions = new string[] { ".mp3", ".wav", ".aif", ".mid" };

        string currentChatName = "";
        int chatMembersCount = 0;       //integer for chat members, to avoid null pointer exception at for below

        public DirectMessage(MainScreen ms)
        {
            //Initialize and load the chat
            InitializeComponent();
            mainScreen = ms;
            firebaseDB = new Firebase(ms);
        }

        public DirectMessage(MainScreen ms, ContactList cl)
        {
            //Initialize and load the chat
            InitializeComponent();
            mainScreen = ms;
            firebaseDB = new Firebase(ms);
            contactList = cl;
        }

        public DirectMessage(string chatIDVal, int type, MainScreen ms)
        {
            //Initialize and load the chat
            InitializeComponent();
            chatID = chatIDVal;
            chatType = type;
            mainScreen = ms;
            firebaseDB = new Firebase(ms);
        }

        public DirectMessage(GroupInfo group, int type, MainScreen ms)
        {
            //Initialize and load the chat
            InitializeComponent();
            groupInfo = group;
            chatType = type;

            mainScreen = ms;
            firebaseDB = new Firebase(ms);
            chatMembers = group.members;
        }

        public DirectMessage(string chatIDVal, int type, MainScreen ms, ContactList cl)
        {
            //Initialize and load the chat
            InitializeComponent();
            chatID = chatIDVal;
            chatType = type;
            mainScreen = ms;
            firebaseDB = new Firebase(ms);
            contactList = cl;
        }

        public DirectMessage(GroupInfo group, int type, MainScreen ms, ContactList cl)
        {
            //Initialize and load the chat
            InitializeComponent();
            groupInfo = group;
            chatType = type;

            mainScreen = ms;
            firebaseDB = new Firebase(ms);
            chatMembers = group.members;
            contactList = cl;

        }

        public void Tick(object sender, EventArgs e)
        {
            RetrieveMessages();
        }

        //Show a text bubble containing a video
        public void AddMessage_Text(string message, string displayName, int role, string date = "")
        {
            //Create a label that makes a gap between messages.
            //It's larger if you don't have the  same role during consecutive messages.
            Label bufferLabel = new Label();

            if (previousRole == role)
                bufferLabel.Height = 1;
            else
                bufferLabel.Height = 5;

            chatbox.Children.Add(bufferLabel);

            SolidColorBrush color = new SolidColorBrush();
            for (int x = 0; x < userColors.Count; x++)
            {
                if (userColors[x].Contains(displayName))
                    color = (SolidColorBrush)userColors[x][1];
            }

            Label top = new Label();
            top.Content = displayName;
            top.FontWeight = FontWeights.Bold;
            top.Foreground = color;
            top.Padding = new Thickness(2);

            TextBlock messageBlock = new TextBlock();
            messageBlock.Inlines.Add(message);
            messageBlock.TextWrapping = TextWrapping.Wrap;
            messageBlock.Padding = new Thickness(3);

            if (date == "")
                date = GetTime();

            Label bottom = new Label();
            bottom.Content = date;
            bottom.FlowDirection = FlowDirection.RightToLeft;
            bottom.Padding = new Thickness(2);
            bottom.FontSize = 10;

            bottom.MinWidth = messageBlock.MinWidth = top.MinWidth = 50;
            bottom.MinHeight = messageBlock.MinHeight = top.MinHeight = 20;
            messageBlock.FontSize = top.FontSize = 14;
            bottom.MaxWidth = messageBlock.MaxWidth = top.MaxWidth = 750;

            StackPanel sp = new StackPanel();
            sp.Children.Add(top);

            //messageBlock.MinWidth = messageBlock.MinWidth + 5;
            sp.Children.Add(messageBlock);
            sp.Children.Add(bottom);

            if (role == (int)Role.Sender)
            {
                sp.HorizontalAlignment = HorizontalAlignment.Right;
                sp.Background = Brushes.Khaki;
            }
            else
            {
                sp.HorizontalAlignment = HorizontalAlignment.Left;
                sp.Background = Brushes.Azure;
            }

            chatbox.Children.Add(sp);
        }

        //Place the image in the chat
        public void AddMessage_Image(string path, string displayName, int role, string date = "")
        {
            if (date == "")
                date = GetTime();

            SolidColorBrush color = new SolidColorBrush();
            for (int x = 0; x < userColors.Count; x++)
            {
                if (userColors[x].Contains(displayName))
                    color = (SolidColorBrush)userColors[x][1];
            }

            ChatImage ci = new ChatImage(displayName, path, date, role, color);

            if (role == (int)Role.Sender)
            {
                ci.HorizontalAlignment = HorizontalAlignment.Right;
                ci.Background = Brushes.Khaki;
            }
            else
            {
                ci.HorizontalAlignment = HorizontalAlignment.Left;
                ci.Background = Brushes.Azure;
            }

            Label bufferLabel = new Label();
            bufferLabel.Background = Brushes.Transparent;

            if (previousRole == role)
                bufferLabel.Height = 1;
            else
                bufferLabel.Height = 5;

            chatbox.Children.Add(bufferLabel);
            chatbox.Children.Add(ci);
        }

        public void AddMessage_Video(string path, string displayName, int role, string date = "")
        {
            if (date == "")
                date = GetTime();

            SolidColorBrush color = new SolidColorBrush();
            for (int x = 0; x < userColors.Count; x++)
            {
                if (userColors[x].Contains(displayName))
                    color = (SolidColorBrush)userColors[x][1];
            }

            ChatVideo cv = new ChatVideo(displayName, path, date, role, color);

            if (role == (int)Role.Sender)
            {
                cv.HorizontalAlignment = HorizontalAlignment.Right;
                cv.Background = Brushes.Khaki;
            }
            else
            {
                cv.HorizontalAlignment = HorizontalAlignment.Left;
                cv.Background = Brushes.Azure;
            }

            Label bufferLabel = new Label();
            bufferLabel.Background = Brushes.Transparent;

            if (previousRole == role)
                bufferLabel.Height = 1;
            else
                bufferLabel.Height = 5;

            chatbox.Children.Add(bufferLabel);
            chatbox.Children.Add(cv);
        }

        public void AddMessage_Audio(string path, string displayName, int role, string date = "")
        {
            if (date == "")
                date = GetTime();

            SolidColorBrush color = new SolidColorBrush();
            for (int x = 0; x < userColors.Count; x++)
            {
                if (userColors[x].Contains(displayName))
                    color = (SolidColorBrush)userColors[x][1];
            }

            ChatAudio ca = new ChatAudio(displayName, path, date, role, color);

            if (role == (int)Role.Sender)
            {
                ca.HorizontalAlignment = HorizontalAlignment.Right;
                ca.Background = Brushes.Khaki;
            }
            else
            {
                ca.HorizontalAlignment = HorizontalAlignment.Left;
                ca.Background = Brushes.Azure;
            }

            Label bufferLabel = new Label();
            bufferLabel.Background = Brushes.Transparent;

            if (previousRole == role)
                bufferLabel.Height = 1;
            else
                bufferLabel.Height = 5;

            chatbox.Children.Add(bufferLabel);
            chatbox.Children.Add(ca);
        }

        //Send a message (not yet functional, also not yet adjusted for images, videos etc.)
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            /*
             * You still need to find a way to discover the other person's ID.
             */

            if (txtMessage.Text != null && txtMessage.Text != "")
            {
                SendMessage_Text();
            }

            if (selectedFilePath != null)
            {
                if (fileType == (int)FileType.Image)
                    SendMessage_Image();
                else if (fileType == (int)FileType.Video)
                    SendMessage_Video();
                else
                    SendMessage_Audio();
            }
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtMessage.Text != null && txtMessage.Text != "")
                {
                    SendMessage_Text();
                }

                if (selectedFilePath != null)
                {
                    if (fileType == (int)FileType.Image)
                        SendMessage_Image();
                    else if (fileType == (int)FileType.Video)
                        SendMessage_Video();
                    else
                        SendMessage_Audio();
                }
            }
        }

        //Encrypt messages
        public string CaesarCipherEncrypt(string plainText, int offset)
        {
            string cipherText = "";

            foreach (char c in plainText)
            {
                cipherText += Convert.ToChar((c + offset));
            }

            return cipherText;
        }

        //Decrypt messages
        public string CaesarCipherDecrypt(string cipherText, int offset)
        {
            string plainText = "";

            foreach (char c in cipherText)
            {
                plainText += Convert.ToChar((c - offset));
            }

            return plainText;
        }

        //Send a text message to the db, appears on the interface
        public void SendMessage_Text()
        {
            //Remove unwanted spaces
            string message = txtMessage.Text.Trim();

            //Add message bubble to chat.
            AddMessage_Text(message, me.displayName, (int)Role.Sender);
            previousRole = (int)Role.Sender;
            txtMessage.Clear();

            //Create message and store it in the DB
            Message m = new Message(CaesarCipherEncrypt(message, encryptionOffset), TimeConversion.DateTimeToUnixTime(DateTime.Now), me.uid);
            myMessages.Add(m);

            if (chatType == 1)
            {
                firebaseDB.Insert($"buddychats/{me.uid}/{chatID}/{myMessages.Count - 1}", m);
                firebaseDB.Insert($"buddychats/{chatID}/{me.uid}/{myMessages.Count - 1}", m);
            }
            else
            {
                firebaseDB.Insert($"groups/{groupInfo.owner}/{groupInfo.groupName}/msgboard/{myMessages.Count - 1}", m);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool internetAvailable;
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadString("https://www.latlmes.com/arts/return-of-the-golden-age-of-comics-1");
                    internetAvailable = true;
                }
                catch
                {
                    internetAvailable = false;
                    txtMessage.IsEnabled = false;
                    btnAttachment.IsEnabled = false;
                    btnSend.IsEnabled = false;
                    btnVoiceNote.IsEnabled = false;
                }

                if (internetAvailable)
                {
                    me = await firebaseDB.GetPerson(Properties.Settings.Default.SignedInID);

                    if (chatType == 1)
                        chatMembers = await GetChatMembersInfo();
                }
                else
                {
                    me = new Person(Properties.Settings.Default.SignedInID);
                    LoadChatMembers();
                }

                DirectoryInfo di = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Compressed");
                if (!di.Exists)
                    di.Create();

                DirectoryInfo di2 = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Decompressed");
                if (!di2.Exists)
                    di2.Create();

                if (chatType == 1)
                    lblChatName.Content = chatMembers[0].displayName;
                else
                    lblChatName.Content = groupInfo.groupName;

                Random rdm = new Random();
                userColors.Add(new List<object>() { me.displayName, presetUserColors[rdm.Next(presetUserColors.Length)] });

                if (chatMembers != null)
                {
                    chatMembersCount = chatMembers.Count;
                }

                for (int x = 0; x < chatMembersCount; x++)
                {
                    userColors.Add(new List<object>() { chatMembers[x].displayName, presetUserColors[rdm.Next(presetUserColors.Length)] });
                }

                try
                {
                    LoadBackup();
                }
                catch
                {
                    myMessages = new List<Message>();
                }

                try
                {
                    RetrieveMessages();

                    //Create timer that performs a database enquery after a set interval
                    timer = new System.Windows.Forms.Timer();
                    timer.Interval = 2500;
                    timer.Tick += new EventHandler(Tick);
                    timer.Start();
                }
                catch
                { }
            }
            catch { }
        }

        public async Task<List<Person>> GetChatMembersInfo()
        {
            List<Person> members;

            members = new List<Person>();
            members.Add(await firebaseDB.GetPerson(chatID));
            return members;
        }

        //Sends images to the db, appears on the interface
        public async void SendMessage_Image()
        {
            //Place image in db, get its url
            string downloadurl = await firebaseDB.SendFile(MessageCompression.Compress(selectedFilePath, me.uid), "Images");

            //Add the image to the chat's interface and the message, with the image url, to the chat's db
            AddMessage_Image(selectedFilePath, me.displayName, (int)Role.Sender);
            Message m = new Message(CaesarCipherEncrypt(downloadurl, encryptionOffset), TimeConversion.DateTimeToUnixTime(DateTime.Now), me.uid);
            myMessages.Add(m);

            if (chatType == 1)
            {
                firebaseDB.Insert($"buddychats/{me.uid}/{chatID}/{myMessages.Count - 1}", m);
                firebaseDB.Insert($"buddychats/{chatID}/{me.uid}/{myMessages.Count - 1}", m);
            }
            else
            {
                firebaseDB.Insert($"groups/{groupInfo.owner}/{groupInfo.groupName}/msgboard/{myMessages.Count - 1}", m);
            }

            selectedFilePath = null;
        }

        //Sends videos to the db, doesn't appear on the interface (yet)
        public async void SendMessage_Video()
        {
            //Place video in db, get its url
            string downloadurl = await firebaseDB.SendFile(MessageCompression.Compress(selectedFilePath, me.uid), "Videos");

            //Add the video to the chat's interface and the message, with the video url, to the chat's db
            AddMessage_Video(selectedFilePath, me.displayName, (int)Role.Sender);
            Message m = new Message(CaesarCipherEncrypt(downloadurl, encryptionOffset), TimeConversion.DateTimeToUnixTime(DateTime.Now), me.uid);
            myMessages.Add(m);

            if (chatType == 1)
            {
                firebaseDB.Insert($"buddychats/{me.uid}/{chatID}/{myMessages.Count - 1}", m);
                firebaseDB.Insert($"buddychats/{chatID}/{me.uid}/{myMessages.Count - 1}", m);
            }
            else
            {
                firebaseDB.Insert($"groups/{groupInfo.owner}/{groupInfo.groupName}/msgboard/{myMessages.Count - 1}", m);
            }

            selectedFilePath = null;
        }

        public void CreateBackup()
        {
            try
            {
                List<Message> backup = new List<Message>();
                backup.AddRange(myMessages);

                FileInfo members;
                if (chatType == 1)
                {
                    DirectoryInfo di = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\{chatID}");
                    if (!di.Exists)
                        di.Create();

                    members = new FileInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\{chatID}\members.fml");

                    if (!members.Exists)
                    {
                        using (Stream str = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\{chatID}\members.fml",
                                                   FileMode.Create, FileAccess.Write))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            List<Person> m = new List<Person>();

                            //MessageBox.Show
                            m.Add(me);
                            m.Add(chatMembers[0]);
                            bf.Serialize(str, m);
                        }
                    }
                }
                else
                {
                    DirectoryInfo di2 = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{groupInfo.owner}\{groupInfo.groupName}");
                    if (!di2.Exists)
                        di2.Create();

                    members = new FileInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{groupInfo.owner}\{groupInfo.groupName}\members.fml");

                    if (!members.Exists)
                    {
                        using (Stream str = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\{groupInfo.owner}\{groupInfo.groupName}\members.fml",
                                                   FileMode.Create, FileAccess.Write))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(str, chatMembers);
                        }
                    }
                }

                string onlinePath,
                       offlineCompressedPath,
                       offlineDecompressedPath;

                for (int x = 0; x < backup.Count; x++)
                {
                    if (backup[x].message.Contains("<|<"))
                    {
                        onlinePath = CaesarCipherDecrypt(backup[x].message, encryptionOffset);

                        string fileName = onlinePath.Substring(onlinePath.IndexOf("%2F") + 3);
                        fileName = fileName.Remove(fileName.IndexOf('?'));
                        fileName = fileName.Replace("%28", "(").Replace("%29", ")");

                        //string fileName = MessageCompression.GetFileName(onlinePath);
                        offlineCompressedPath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Compressed\{fileName}";
                        offlineDecompressedPath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Decompressed\{fileName}";

                        FileInfo fi = new FileInfo(offlineCompressedPath);
                        if (!fi.Exists)
                        {
                            WebClient webClient = new WebClient();
                            webClient.DownloadFile(onlinePath, $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Compressed\{fileName}");
                        }


                        FileInfo fi2 = new FileInfo(offlineDecompressedPath);

                        if (!fi2.Exists)
                            MessageCompression.Decompress($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Compressed\{fileName}", me.uid);


                        backup[x].message = CaesarCipherEncrypt(offlineDecompressedPath, encryptionOffset);
                    }
                }

                string path = "";
                if (chatType == 1)
                {
                    path = $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\{chatID}\{chatID}.fml";
                }
                else
                {
                    path = $@"{AppDomain.CurrentDomain.BaseDirectory}\{groupInfo.owner}\{groupInfo.groupName}\{groupInfo.groupName}.fml";
                }

                using (Stream str = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(str, backup);
                }
            }
            catch { }
        }
        //

        public void LoadBackup()
        {
            string backuppath = "";

            if (chatType == 1)
                backuppath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\{chatID}\{chatID}.fml";
            else
                backuppath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{groupInfo.owner}\{groupInfo.groupName}\{groupInfo.groupName}.fml";

            using (Stream str = new FileStream(backuppath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                myMessages = bf.Deserialize(str) as List<Message>;
            }

            for (int x = 0; x < myMessages.Count; x++)
            {
                Message m = myMessages[x];
                DisplayMessage(m);
            }
        }

        public void LoadChatMembers()
        {
            string path = "";

            if (chatType == 1)
                path = $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\{chatID}\members.fml";
            else
                path = $@"{AppDomain.CurrentDomain.BaseDirectory}\{groupInfo.owner}\{groupInfo.groupName}\members.fml";

            using (Stream str = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                chatMembers = bf.Deserialize(str) as List<Person>;

                if (chatMembers != null)
                {
                    chatMembersCount = chatMembers.Count;
                }

                for (int x = 0; x < chatMembersCount; x++)
                {
                    if (chatMembers[x].uid == Properties.Settings.Default.SignedInID)
                    {
                        me = new Person(chatMembers[x].displayName, chatMembers[x].photoURL, chatMembers[x].uid);

                        if (chatType == 1)
                        {
                            chatMembers.RemoveAt(0);
                            chatMembers.Add(me);
                        }

                        break;
                    }
                }
            }
        }

        //Sends audio to the db, doesn't appear on the interface (yet)
        public async void SendMessage_Audio()
        {
            //Place audio in db, get its url
            string downloadurl = await firebaseDB.SendFile(MessageCompression.Compress(selectedFilePath, me.uid), "Audio");

            //Add the audio to the chat's interface and the message, with the audio url, to the chat's db
            AddMessage_Audio(selectedFilePath, me.displayName, (int)Role.Sender);
            Message m = new Message(CaesarCipherEncrypt(downloadurl, encryptionOffset), TimeConversion.DateTimeToUnixTime(DateTime.Now), me.uid);
            myMessages.Add(m);

            if (chatType == 1)
            {
                firebaseDB.Insert($"buddychats/{me.uid}/{chatID}/{myMessages.Count - 1}", m);
                firebaseDB.Insert($"buddychats/{chatID}/{me.uid}/{myMessages.Count - 1}", m);
            }
            else
            {
                firebaseDB.Insert($"groups/{groupInfo.owner}/{groupInfo.groupName}/msgboard/{myMessages.Count - 1}", m);
            }

            selectedFilePath = null;
        }

        private async void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            List<Person> friends = new List<Person>();

            currentChatName = lblChatName.Content.ToString();

            if (MessageBox.Show("Show only friends?", "Only Friends?", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                friends = await contactList.GetFriends();
            }
            else
            {
                friends = await contactList.Get_Firebase().GetAllUsers();

                friends.Remove(me);
            }

            mainScreen.Get_TrayBalloon().Show_Friends_Menu(friends, this);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            CreateBackup();
        }

        public async void RetrieveMessages()
        {
            //Get the messages from a certain chat
            if (chatType == 1)
            {
                newMessages = await firebaseDB.GetMessages($"buddychats/{me.uid}", chatID);
            }
            else
                newMessages = await firebaseDB.GetMessages($"groups/{groupInfo.owner}/{groupInfo.groupName}/msgboard", chatID);

            //For all new messages, create a text bubble on the chat interface.
            if (newMessages != null)
            {
                try
                {
                    newMessages.RemoveRange(0, myMessages.Count);
                }
                catch
                {
                    newMessages.RemoveRange(0, myMessages.Count - 1);
                }

                for (int x = 0; x < newMessages.Count; x++)
                {
                    try
                    {
                        Message m = newMessages[x];
                        myMessages.Add(m);
                        DisplayMessage(m);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.StackTrace);
                    }
                }

                if (newMessages.Count > 0)  //This is still not working
                {
                    mainScreen.Get_TrayBalloon().Show_Notification("New Messages", "You have Bread Mail!");
                }

                //Add the new messages to your message list and clear the new messages
                newMessages.Clear();
            }
        }

        public void DisplayMessage(Message m)
        {
            int role;
            string personDisplayName = "";

            //If you were the sender of the message, you are flagged as such
            if (m.sentby == me.uid)
            {
                role = (int)Role.Sender;
                personDisplayName = me.displayName;
            }
            else
            {
                role = (int)Role.Receiver;

                if (chatType == 1)
                {
                    personDisplayName = chatMembers[0].displayName;
                }
                else
                {
                    if (chatMembers != null)
                    {
                        chatMembersCount = chatMembers.Count;
                    }

                    for (int x = 0; x < chatMembersCount; x++)
                    {
                        if (chatMembers[x].uid == m.sentby)
                        {
                            personDisplayName = chatMembers[x].displayName;
                            break;
                        }
                    }
                }
            }

            //Still need to implement this part
            string decryptedContent = CaesarCipherDecrypt(m.message, encryptionOffset);
            string timestamp = TimeConversion.UnixTimeToDateTime(m.timestamp).ToString("dd/MM, HH:mm");

            if (decryptedContent.Contains(@"/o/"))
            {
                string fileName = decryptedContent.Substring(decryptedContent.IndexOf("%2F") + 3);
                fileName = fileName.Remove(fileName.IndexOf('?'));
                fileName = fileName.Replace("%28", "(").Replace("%29", ")");

                FileInfo fi = new FileInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Compressed\{fileName}");

                if (!fi.Exists)
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(decryptedContent, $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Compressed\{fileName}");
                }

                FileInfo fi2 = new FileInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Decompressed\{fileName}");

                if (!fi2.Exists)
                    decryptedContent = MessageCompression.Decompress($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Compressed\{fileName}", me.uid);

                decryptedContent = $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\Decompressed\{fileName}";
                string extention = "." + decryptedContent.Substring(decryptedContent.LastIndexOf('(') + 1, decryptedContent.LastIndexOf(')') - decryptedContent.LastIndexOf('(') - 1);
                decryptedContent = decryptedContent.Remove(decryptedContent.LastIndexOf('(')).Replace(@"\\", @"\") + extention;

                if (imageExtensions.Contains(extention))
                    AddMessage_Image(decryptedContent, personDisplayName, role, timestamp);
                else if (videoExtensions.Contains(extention))
                    AddMessage_Video(decryptedContent, personDisplayName, role, timestamp);
                else if (audioExtensions.Contains(extention))
                    AddMessage_Audio(decryptedContent, personDisplayName, role, timestamp);
            }
            else if (decryptedContent.Contains(@"\Decompressed\"))
            {
                string extention = "." + decryptedContent.Substring(decryptedContent.LastIndexOf('(') + 1, decryptedContent.LastIndexOf(')') - decryptedContent.LastIndexOf('(') - 1);
                decryptedContent = decryptedContent.Remove(decryptedContent.LastIndexOf('(')).Replace(@"\\", @"\") + extention;

                if (imageExtensions.Contains(extention))
                    AddMessage_Image(decryptedContent, personDisplayName, role, timestamp);
                else if (videoExtensions.Contains(extention))
                    AddMessage_Video(decryptedContent, personDisplayName, role, timestamp);
                else if (audioExtensions.Contains(extention))
                    AddMessage_Audio(decryptedContent, personDisplayName, role, timestamp);
            }
            else
                AddMessage_Text(decryptedContent, personDisplayName, role, timestamp);
        }

        private void BtnVoiceNote_Click(object sender, RoutedEventArgs e)
        {
            vnClickCount++;

            //Record
            if (vnClickCount % 2 == 1)
            {
                mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
                mciSendString("record recsound", "", 0, 0);
                btnVoiceNote.Background = Brushes.Green;
            }
            //Save file and stop recording, choose file as file to be sent
            else
            {
                string time = DateTime.Now.ToString("yyMMddHHmmss");

                DirectoryInfo di = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\VN");
                if (!di.Exists)
                    di.Create();

                selectedFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\{me.uid}\VN\{time}.wav";
                string command = "save recsound " + selectedFilePath;
                mciSendString(command, "", 0, 0);
                mciSendString("close recsound", "", 0, 0);

                btnVoiceNote.Background = Brushes.White;
            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            //Open a window to choose a file from
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

                    //If the file has a valid extension, determine its file type
                    if (imageExtensions.Contains(extension.ToLower()))
                    {
                        fileType = (int)FileType.Image;
                        validExtension = true;
                    }
                    else if (videoExtensions.Contains(extension.ToLower()))
                    {
                        fileType = (int)FileType.Video;
                        validExtension = true;
                    }
                    else if (audioExtensions.Contains(extension.ToLower()))
                    {
                        fileType = (int)FileType.Audio;
                        validExtension = true;
                    }

                    //Save the file's path if it's valid, else give an error message
                    if (validExtension)
                        selectedFilePath = file.FileName;
                    else
                        mainScreen.Get_TrayBalloon().Show_Notification("Invalid file", "This file type is not supported");
                }
            }
        }

        //Literally just returns the time. I am to lazy to keep typing it out. Also, modularity is a thing
        public string GetTime()
        {
            return DateTime.Now.ToString("dd/MM, HH:mm");
        }

        public string Get_Current_Chat_Name()
        {
            return currentChatName;
        }
    }
}
