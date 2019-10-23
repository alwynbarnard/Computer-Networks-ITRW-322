using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop
{
    public class Firebase
    {
        //Setup Firebase path and access rights
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "WDDZLj3T1nqIv634DKpnS2TRgQwcjNBqYj89TYh2",
            BasePath = "https://networksdb-25815.firebaseio.com/"
        };

        IFirebaseClient client;

        public Firebase()
        {
            //Connect to the Firebase database
            client = new FireSharp.FirebaseClient(config);

            //If the client can't connect to the DB, notify the user
            if (client == null)
            {
                MessageBox.Show("You are currently offline, please reconnect to the internet in order to continue using Brood.NET.",
                                "No internet access",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        //Insert an object into the database
        //Storage location is the full file path of the object, except for the object's name
        
        public async void Insert(object dataObject, string storageLocation, string dataObjectName)
        {
            try
            {
                SetResponse response = await client.SetTaskAsync($"{storageLocation}/{dataObjectName}", dataObject);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }      

        //Retrieve an object from the database
        //Storage location is the full file path of the object, except for the object's name
        public async void Get(string storageLocation, string dataObjectName)
        {
            try
            {
                //FirebaseResponse response = await client.GetTaskAsync($"{storageLocation}/{dataObjectName}");



                //Have to adjust for each object (probably)



            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //Update an object that is within the database
        //Storage location is the full file path of the object, except for the object's name
        public async void Update(object newObject, string storageLocation, string dataObjectName)
        {
            try
            {
                FirebaseResponse response = await client.UpdateTaskAsync($"{storageLocation}/{dataObjectName}", newObject);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //Delete an object that is within the database
        //Storage location is the full file path of the object, except for the object's name
        public async void Delete(string storageLocation, string dataObjectName)
        {
            try
            {
                FirebaseResponse response = await client.DeleteTaskAsync($"{storageLocation}/{dataObjectName}");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
