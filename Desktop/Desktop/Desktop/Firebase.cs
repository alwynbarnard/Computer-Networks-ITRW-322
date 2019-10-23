using Firebase.Storage;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop
{
    public class Firebase
    {
        private MainScreen mainScreen;

        static string basePath = Properties.Settings.Default.FirebaseAppUri;
        static string storagePath = Properties.Settings.Default.FirebaseStorage;

        //Setup Firebase path and access rights
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = Properties.Settings.Default.NormalFireKey,
            BasePath = basePath
        };

        IFirebaseClient client;

        public Firebase(MainScreen ms)
        {
            mainScreen = ms;

            //Connect to the Firebase database
            client = new FireSharp.FirebaseClient(config);

            //If the client can't connect to the DB, notify the user
            if (client == null)
            {
            }
        }

        //Insert an object into the database
        //Storage location is the full file path of the object, except for the object's name
        
        public async void Insert(string storageLocation, object dataObject)
        {
            try
            {
                SetResponse response = await client.SetTaskAsync(storageLocation, dataObject);
            }
            catch (Exception e)
            {
            }
        }


        public async void InsertFriend(string storageLocation, string uid)
        {
            try
            {
                SetResponse response = await client.SetTaskAsync(storageLocation, uid);
            }
            catch (Exception e)
            {
            }
        }

        //Retrieve an object from the database
        //Storage location is the full file path of the object, except for the object's name
        public async Task<List<Message>> GetMessages(string storageLocation, string dataObjectName)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"{storageLocation}/{dataObjectName}");
                List<Message> messages = response.ResultAs<List<Message>>();
                return messages;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Person> GetPerson(string id)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"users/{id}");
                Person p = response.ResultAs<Person>();
                return p;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Person>> GetAllUsers()
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"users");

                List<Person> allUsers = new List<Person>();
                string responseBody = response.Body;

                while (responseBody.Contains("displayName"))
                {
                    int begin = responseBody.LastIndexOf("uid\":") + 6;
                    int end = responseBody.LastIndexOf("\"}");

                    allUsers.Add(await GetPerson(responseBody.Substring(begin, end - begin)));

                    responseBody = responseBody.Remove(responseBody.LastIndexOf("\"displayName\":"));
                }
                
                return allUsers; 
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Person>> GetAllFriends(string uid)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"friends/{uid}");
                List<Person> friendIDs = response.ResultAs<List<Person>>();
                List<Person> friends = new List<Person>();
                
                for (int x = 0; x < friendIDs.Count; x++)
                {
                    FirebaseResponse userResponse = await client.GetTaskAsync($"users/{friendIDs[x].uid}");
                    Person p = userResponse.ResultAs<Person>();
                    friends.Add(p);
                }

                return friends;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<GroupInfo> GetUsersGroupInfo(string userUID, string groupName)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"groups/{userUID}/{groupName}");
                GroupInfo groupInfo = new GroupInfo(); 
                groupInfo = response.ResultAs<GroupInfo>();

                return groupInfo;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<GroupInfo>> GetUsersGroups(string uid)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"groupsnames/{uid}");
                List<GroupInfo> groupInfo = response.ResultAs<List<GroupInfo>>();


                for (int x = 0; x < groupInfo.Count; x++)
                {
                    FirebaseResponse res = await client.GetTaskAsync($"groups/{groupInfo[x].owner}/{groupInfo[x].groupName}/members/");
                    groupInfo[x].members = res.ResultAs<List<Person>>();

                    FirebaseResponse owner = await client.GetTaskAsync($"users/{groupInfo[x].owner}");
                    groupInfo[x].members.Add(owner.ResultAs<Person>());
                }

                return groupInfo;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async void InsertGroup(GroupInfo group, string uid)
        {
            try
            {
                GroupInfo insertGroup = new GroupInfo
                {
                    groupimage = group.groupimage,
                    members = group.members,
                    msgboard = new List<Message>(),
                    owner = uid
                };

                SetResponse response = await client.SetTaskAsync($"groups/{uid}/{group.groupName}", insertGroup);

                Group thisGroup = new Group();
                thisGroup.groupName = group.groupName;
                thisGroup.owner = uid;

                InsertGroupName(thisGroup, uid);
            }
            catch (Exception e)
            {
            }
        }

        public async Task<List<Group>> GetGroupNames(string uid)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"groupsnames/{uid}");
                List<Group> groups = response.ResultAs<List<Group>>();

                return groups;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async void InsertGroupName(Group group, string uid)
        {
            try
            {
                List<Group> existingGroups = new List<Group>();
                existingGroups = await GetGroupNames(uid);
                int count = 0;

                if (existingGroups != null)
                {
                    count = existingGroups.Count;
                }

                SetResponse response = await client.SetTaskAsync($"groupsnames/{uid}/{count}", group);
            }
            catch (Exception e)
            {
            }
        }

        public async Task<List<CommandObject>> GetAllCommands(string uid)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"commands/{uid}");
                List<CommandObject> commandList = response.ResultAs<List<CommandObject>>();

                return commandList;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public async Task<List<Person>> GetGroupMembers(string uid)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"friends/{uid}");
                List<Person> friendIDs = response.ResultAs<List<Person>>();

                List<Person> friends = new List<Person>();

                for (int x = 0; x < friendIDs.Count; x++)
                {
                    FirebaseResponse userResponse = await client.GetTaskAsync($"users/{friendIDs[x].uid}");
                    Person p = userResponse.ResultAs<Person>();
                    friends.Add(p);
                }

                return friends;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Person> GetInfo(string storageLocation, string dataObjectName)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"{storageLocation}/{dataObjectName}");
                Person person = response.ResultAs<Person>();
                return person;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        //Update an object that is within the database
        //Storage location is the full file path of the object, except for the object's name
        public async void Update(string storageLocation, object newObject)
        {
            try
            {
                FirebaseResponse response = await client.UpdateTaskAsync(storageLocation, newObject);
            }
            catch (Exception e)
            {
                //mainScreen.Get_TrayBalloon().Show_Notification("Update error", e.Message);
            }
        }

        public async Task<List<Person>> GetFriendRequests(string uid)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"requests/{uid}");
                List<FriendRequest> requestList = response.ResultAs<List<FriendRequest>>();
                List<Person> people = new List<Person>();

                for (int i = 0; i < requestList.Count; i++)
                {
                    FirebaseResponse userResponse = await client.GetTaskAsync($"users/{requestList[i].sender}");
                    Person p = userResponse.ResultAs<Person>();
                    people.Add(p);
                }

                return people;
            }
            catch (Exception e)
            {
                //mainScreen.Get_TrayBalloon().Show_Notification("Get All Commands error", e.Message);
                return null;
            }
        }

        public async Task<List<FriendRequest>> GetAllRequests(string uid)
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync($"requests/{uid}");
                List<FriendRequest> requestList = response.ResultAs<List<FriendRequest>>();
                
                return requestList;
            }
            catch (Exception e)
            {
                //mainScreen.Get_TrayBalloon().Show_Notification("Get All Commands error", e.Message);
                return null;
            }
        }

        public async void DeleteCommand(CommandObject command)
        {
            try
            {
                string myUID = Properties.Settings.Default.SignedInID;
                List<CommandObject> commandList = new List<CommandObject>();
                commandList = await GetAllCommands(myUID);
                int commandListCount = 0;

                if (commandList != null)
                {
                    commandListCount = commandList.Count;
                }

                for (int i = 0; i < commandListCount; i++)
                {
                    if (commandList[i].Command == command.Command)
                    {
                        FirebaseResponse response = await client.DeleteTaskAsync($"commands/{myUID}/{i}");

                        i = commandListCount;
                    }
                }
            }
            catch (Exception e)
            {
                //mainScreen.Get_TrayBalloon().Show_Notification("Get All Commands error", e.Message);
            }

        }
        public async void DeleteFriendRequest(FriendRequest request)
        {
            try
            {
                string myUID = Properties.Settings.Default.SignedInID;
                List<FriendRequest> requests = new List<FriendRequest>();
                requests = await GetAllRequests(myUID);

                for (int i = 0; i < requests.Count; i++)
                {
                    if (requests[i].sender == request.sender)
                    {
                        FirebaseResponse response = await client.DeleteTaskAsync($"requests/{myUID}/{i}");
                        i = requests.Count;
                    }
                }
            }
            catch (Exception e)
            {
                //mainScreen.Get_TrayBalloon().Show_Notification("Get All Commands error", e.Message);
            }
        }

        //Delete an object that is within the database
        //Storage location is the full file path of the object, except for the object's name
        public async void Delete(string storageLocation, string dataObjectName)
        {
            /*
            try
            {
                FirebaseResponse response = await client.DeleteTaskAsync($"{storageLocation}/{dataObjectName}");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            */ 


            //This is still buggy, will be fixed at a later stage



        }

        //Send a image, video or other file
        public async Task<string> SendFile(string path, string targetFirebaseFolder)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //Get the file's name
                string filename = path.Substring(path.LastIndexOf(@"\") + 1).Replace(' ','_');

                //Construct FirebaseStorage, path to where you want to upload the file and Put it there
                var task = new FirebaseStorage(storagePath)
                           .Child(targetFirebaseFolder)
                           .Child(filename)
                           .PutAsync(stream);

                //Await the task to wait until upload completes and get the download url
                var downloadUrl = await task;
                return downloadUrl.ToString();
            }
        }
    }
}
