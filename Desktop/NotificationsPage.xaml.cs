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
    /// Interaction logic for NotificationsPage.xaml
    /// </summary>
    public partial class NotificationsPage : Page
    {
        bool conversationTones = false;
        bool messagesVibrate = false;
        bool groupsVibrate = false;
        bool callsVibrate = false;

        public NotificationsPage()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbxConversationTones.IsChecked == true)
            {
                conversationTones = true;
                MessageBox.Show("conversation tone checked");
            }

            if (cbxMessageVibrate.IsChecked == true)
            {
                messagesVibrate = true;
                MessageBox.Show("messages vibrate checked");
            }

            if (cbxGroupVibrate.IsChecked == true)
            {
                groupsVibrate = true;
                MessageBox.Show("groups vibrate checked");
            }

            if (cbxCallVibrate.IsChecked == true)
            {
                callsVibrate = true;
                MessageBox.Show(" Calls vibrate checked");
            }

            MessageBox.Show("Changes saved successfully!");

            
        }
    }
}
