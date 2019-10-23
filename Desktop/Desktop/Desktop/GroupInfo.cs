using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop
{
    public class GroupInfo
    {
        public string groupName { get; set; }
        public string groupimage { get; set; }
        public List<Person> members;
        public List<Message> msgboard { get; set; }
        public string owner { get; set; }

        public GroupInfo()
        {

        }

        public GroupInfo(string groupname, string groupowner)
        {
            groupName = groupname;
            owner = groupowner;
        }

        public GroupInfo(string groupname, List<Person> groupmembers, string groupowner)
        {
            groupName = groupname;
            members = groupmembers;
            owner = groupowner;
        }

        public GroupInfo(string groupname, List<Person> groupmembers, string groupowner, string image)
        {
            groupName = groupname;
            members = groupmembers;
            owner = groupowner;
            groupimage = image;
        }
    }
}
