using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop
{
    public class Chat
    {
        //Properties related to the Chat Class
        public int Chat_ID { get; set; }
        public List<Message> Messages { get; set; }
        public List<string> Usernames { get; set; }

        //Create a chat object resembling a Firebase data entry
        public Chat(int chat_id, List<Message> messages, List<string> usernames)
        {
            Chat_ID = chat_id;
            Messages = messages;
            Usernames = usernames;
        }
    }
}
