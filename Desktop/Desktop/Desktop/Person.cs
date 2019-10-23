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
    public class Person
    {
        //Properties related to the Person Class
        public string displayName { get; set; }
        public string photoURL { get; set; }
        public string uid { get; set; }

        public Person()
        {

        }

        public Person(string id)
        {
            uid = id;
        }

        //Create a chat object resembling a Firebase data entry
        public Person(string displayName, string pic, string id)
        {
            this.displayName = displayName;
            photoURL = pic;
            uid = id;
        }

        //Obtain this device's IP address
        public static string GetLocalIPAddress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }
    }
}
