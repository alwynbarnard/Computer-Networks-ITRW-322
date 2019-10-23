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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainScreen : Window
    {
        public MainScreen()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            DirectMessage dm = new DirectMessage();
            frame.Navigate(dm);
        }

        private void BtnVoiceCall_Click(object sender, RoutedEventArgs e)
        {   //Changes the size of the Voice call to show that it is active
            /*btnVideoCall.Opacity = 0;
            btnVoiceCall.Width = 235;
            btnVoiceCall.IsEnabled = false;*/
            //PageVoicCall vc = new PageVoicCall();
            //frame.Navigate(vc);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {   //Resets the two call buttons
            /*btnVoiceCall.Width = 117;
            btnVoiceCall.Opacity = 100;
            btnVoiceCall.IsEnabled = true;
            btnVoiceCall.Margin = new Thickness(10, 10, 0, 10);

            btnVideoCall.Width = 117;
            btnVideoCall.Opacity = 100;
            btnVideoCall.IsEnabled = true;
            btnVideoCall.Margin = new Thickness(140, 10, 0, 10);*/

            DirectMessage dm = new DirectMessage();
            frame.Navigate(dm);
        }

        private void BtnVideoCall_Click(object sender, RoutedEventArgs e)
        {//Changes the size of the Voice call to show that it is active
         /* btnVoiceCall.Opacity = 0;
          btnVideoCall.Width = 235;
          btnVideoCall.IsEnabled = false;

          PageVideoCall vd = new PageVideoCall();
          frame.Navigate(vd);

          btnVideoCall.Margin = new Thickness(10, 10, 0, 0);      */
        }
        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        { //Button to display the Account Settings
            //AccountPage accountpage = new AccountPage();
            //frame.Navigate(accountpage);
        }

        private void BtnChatSettings_Click(object sender, RoutedEventArgs e)
        { //Button to display the Chat Settings
            //ChatSettingsPage chatsettingspage = new ChatSettingsPage();
            //frame.Navigate(chatsettingspage);
        }

        private void BtnNotifications_Click(object sender, RoutedEventArgs e)
        { //Button to display the Notification Settings
            //NotificationsPage notificationspage = new NotificationsPage();
            //frame.Navigate(notificationspage);
        }

        private void BtnStorage_Data_Click(object sender, RoutedEventArgs e)
        { //Button to display the Staorage Data and Settings
            //StorageAndDataPage storageanddatapage = new StorageAndDataPage();
            //frame.Navigate(storageanddatapage);
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        { //Button to display the Help info
            //HelpPage helppage = new HelpPage();
            //frame.Navigate(helppage);
        }

        void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (CallsTab.IsSelected)
            {
                CallsList list = new CallsList();

                list.Height = frame.Height - 10;
                list.Width = frame.Width - 10;
                frame.Navigate(list);
            }
			*/
        }

        private void Btn_ContactList_Click(object sender, RoutedEventArgs e)
        {
            //This button will bring up the contact list page
            //ContactList contactlist = new ContactList();
            //frame.Navigate(contactlist);
        }
    }
}
