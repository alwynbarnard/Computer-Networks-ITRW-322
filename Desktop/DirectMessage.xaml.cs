using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
    public partial class DirectMessage : Page
    {
        //If the person is receiving a text, they are labeled a receiver (index value 1).
        //If they are sending a text, they are labeled a sender (index value 2).
        enum Role { Sender = 1, Receiver }
        int previousRole = (int)Role.Sender;
        public DirectMessage()
        {
            InitializeComponent();
        }

        public TextBlock FormatTextBubble(int role)
        {
            //Basic formatting; Text also flows over to next line if it's too large
            TextBlock tb = new TextBlock();
            tb.MinWidth = 30;
            tb.MinHeight = 20;
            tb.FontSize = 14;
            tb.MaxWidth = 750;

            tb.Foreground = Brushes.Black;
            tb.Padding = new Thickness(3.5);
            tb.TextWrapping = TextWrapping.Wrap;

            //Messages from a sender are right aligned; reader's messages are left aligned.
            if (role == (int)Role.Sender)
            {
                tb.HorizontalAlignment = HorizontalAlignment.Right;
                tb.Background = Brushes.Khaki;
            }
            else
            {
                tb.HorizontalAlignment = HorizontalAlignment.Left;
                tb.Background = Brushes.Azure;
            }

            //Create a label that makes a gap between messages.
            //It's larger if you don't have the  same role during consecutive messages.
            Label bufferLabel = new Label();

            if (previousRole == role)
                bufferLabel.Height = 1;
            else
                bufferLabel.Height = 5;

            chatbox.Children.Add(bufferLabel);
            return tb;
        }

        //Send a text bubble in which the sender text will be shown (not yet functional).
        public void AddMessage(string message, int role)
        {
            //Add name, message and time
            TextBlock textBubble = FormatTextBubble(role);
            textBubble.Inlines.Add(new Run("User Name") { Foreground = Brushes.Green });
            textBubble.Inlines.Add("\n" + message);
            textBubble.Inlines.Add(new Run($"\n\t{DateTime.Now.ToString("HH:mm")}") { FontSize = 10 });

            chatbox.Children.Add(textBubble);
        }

        //Send a message (not yet functional, also not yet adjusted for images, videos etc.)
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtMessage.Text != null)
            {
                string message = txtMessage.Text.Trim();
                AddMessage(message, (int)Role.Sender);
                previousRole = (int)Role.Sender;
                txtMessage.Clear();
            }
        }

        //Send a message (not yet adjusted for images, videos etc.) by pressing <Enter> while on the textbox
        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string message = txtMessage.Text.Trim();
                AddMessage(message, (int)Role.Sender);
                previousRole = (int)Role.Sender;
                txtMessage.Clear();
            }
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
