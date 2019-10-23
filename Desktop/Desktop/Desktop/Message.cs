using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Desktop
{
    [Serializable]
    public class Message
    {
        //Properties related to the Message Class
        public string message { get; set; }
        public string sentby { get; set; }
        public double timestamp { get; set; }

        //No parameters
        public Message()
        {

        }

        //Construct a message entry resembling a Firebase entry
        public Message(string content, double dateTimeReleased, string username)
        {
            message = content;
            timestamp = dateTimeReleased;
            sentby = username;
        }
    }
}
