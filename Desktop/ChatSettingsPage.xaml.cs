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
    /// Interaction logic for ChatSettingsPage.xaml
    /// </summary>
    public partial class ChatSettingsPage : Page
    {
        int textSize = 0;
        bool enterSend = false;
        public ChatSettingsPage()
        {
            InitializeComponent();
        }

        private void BtnNewWallpaper_Click(object sender, RoutedEventArgs e)  // EDIT
        {

        }

        private void BtnChatHistory_Click(object sender, RoutedEventArgs e)  // EDIT
        {

        }

        private void BtnChatBackup_Click(object sender, RoutedEventArgs e)  // EDIT
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)  // EDIT
        {
            textSize = Convert.ToInt16(txtTextSize.Text);  //Change Text size
            if (cbxEnter.IsChecked == true)
            {
                enterSend = true;
            }
        }
    }
}
