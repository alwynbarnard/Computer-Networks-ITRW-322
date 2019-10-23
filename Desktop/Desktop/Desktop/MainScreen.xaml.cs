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
        public TrayBalloon trayBalloon;
        public Uri previousPage;

        public MainScreen()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            Login login = new Login(this);

            trayBalloon = new TrayBalloon(this);
            trayBalloon.Start_Tray_Balloon();

            frame.Navigate(login);
        }

        public Frame Get_Frame()
        {
            return frame;
        }

        public TrayBalloon Get_TrayBalloon()
        {
            return trayBalloon;
        }

        private void BtnVoiceCall_Click(object sender, RoutedEventArgs e)
        {   
            //Changes the size of the Voice call to show that it is active
            btnVideoCall.Opacity = 0;
            btnVoiceCall.Width = 235;
            btnVoiceCall.IsEnabled = false;
            previousPage = frame.NavigationService.CurrentSource;
            VoiceCall vc = new VoiceCall(this);
            frame.Navigate(vc);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {   
            //Resets the two call buttons
            btnVoiceCall.Width = 117;
            btnVoiceCall.Opacity = 100;
            btnVoiceCall.IsEnabled = true;
            btnVoiceCall.Margin = new Thickness(10, 10, 0, 10);

            btnVideoCall.Width = 117;
            btnVideoCall.Opacity = 100;
            btnVideoCall.IsEnabled = true;
            btnVideoCall.Margin = new Thickness(140, 10, 0, 10);

            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            frame.Navigate(previousPage);
            frame.NavigationService.RemoveBackEntry();
        }

        private void BtnVideoCall_Click(object sender, RoutedEventArgs e)
        {
            //Changes the size of the Voice call to show that it is active
          btnVoiceCall.Opacity = 0;
          btnVideoCall.Width = 235;
          btnVideoCall.IsEnabled = false;

          VideoCall vd = new VideoCall();
          frame.Navigate(vd);

          btnVideoCall.Margin = new Thickness(10, 10, 0, 0);      
        }
        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        { 

        }

        private void BtnChatSettings_Click(object sender, RoutedEventArgs e)
        { 
        }

        private void BtnNotifications_Click(object sender, RoutedEventArgs e)
        { 
        }

        private void BtnStorage_Data_Click(object sender, RoutedEventArgs e)
        { 
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        { 

        }

        void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void Btn_ContactList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DMs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContactList clDM = new ContactList(this, 1);
            frameDMList.Navigate(clDM);
        }

        private void Groups_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContactList clGr = new ContactList(this, 2);
            frameGroupChats.Navigate(clGr);
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            frame.NavigationService.RemoveBackEntry();
        }

        private void FrameDMList_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            frameDMList.NavigationService.RemoveBackEntry();
        }

        private void FrameGroupChats_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            frameGroupChats.NavigationService.RemoveBackEntry();
        }
    }
}
