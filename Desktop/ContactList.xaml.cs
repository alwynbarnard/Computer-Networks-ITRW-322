//Coded by Zander Boonzaaier 28749995
//Perspective, my dude
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
    /// Interaction logic for ContactList.xaml
    /// This is the base layout for the Contact List Page.
    /// This page will be changed at a later time.
    /// </summary>
    public partial class ContactList : Page
    {
        //VARIABLES USED START
        int iAmmount = 0; //This value is the ammount of contacts that you have, to be recieved from the server
        List<string> lstNames = new List<string>();//This list will be used to dynamically store the names/information of users that are accosiated with the subject user
        //VARIABLES USED END


        //CallsItem[] item = new CallsItem[10];
        Uri[] uris = new Uri[6];
        BitmapImage[] images = new BitmapImage[6];



        public ContactList()
        {
            InitializeComponent();

            //REDUNDANT CODE TO POPULATE LIST START (This code can be removed during database implementation)
            lstNames.Add("André Bruwer");
            lstNames.Add("Alwyn Barnard");
            lstNames.Add("Christiaan Koch");
            lstNames.Add("Cornelius Frylinck");
            lstNames.Add("Gerard Jooster");
            lstNames.Add("Ian Stamatiou");
            lstNames.Add("Juan Swanepoel");
            lstNames.Add("Lloyd Janse van Rensburg");
            lstNames.Add("Michael Botha");
            lstNames.Add("Pieter Verster");
            lstNames.Add("Zander Boonzaaier");
            //REDUNDANT CODE TO POPULATE LIST END

            try
            {
                Add_Pics_To_List();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error adding pics to list", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            iAmmount = 0; //Here we assign the ammount of contacts to iAmmount trough the database
            for (int k = 0; k < iAmmount; k = k + 1)
            {
                AddItem(k % 5, lstNames[k % 11]);
            }
        }

        //Add Images To Each Contact... Will later be implemented to show each individual's own profile picture
        public void Add_Pics_To_List()
        {
            uris[0] = new Uri("E:\\pic1.png");
            images[0] = GetImage(uris[0]);

            uris[1] = new Uri("E:\\pic2.png");
            images[1] = GetImage(uris[1]);

            uris[2] = new Uri("E:\\pic3.jpg");
            images[2] = GetImage(uris[2]);

            uris[3] = new Uri("E:\\pic4.jpg");
            images[3] = GetImage(uris[3]);

            uris[4] = new Uri("E:\\pic5.jpg");
            images[4] = GetImage(uris[4]);

            uris[5] = new Uri("E:\\pic6.jpg");
            images[5] = GetImage(uris[5]);
        }

        //Get an image with a given uri and return as BitmapImage
        public BitmapImage GetImage(Uri uri)
        {
            BitmapImage img = new BitmapImage();

            img.BeginInit();
            img.UriSource = uri;
            //img.DecodePixelHeight = 10;
            //img.DecodePixelWidth = 10;
            img.EndInit();

            return img;
        }

        //Add item to the listbox (Calls List)
        public void AddItem(int index, string name)
        {
            StackPanel panel = new StackPanel();
            Button img = new Button();
            Label nameLabel = new Label();

            img.IsHitTestVisible = false;
            img.Background = new ImageBrush(images[index]);
            img.FontStretch = FontStretches.Condensed;
            img.Height = 80;
            img.Width = 80;

            nameLabel.Content = name;
            nameLabel.FontFamily = new FontFamily("Lucida Sans");
            nameLabel.FontSize = 35;

            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(img);
            panel.Children.Add(nameLabel);

            ListBox1.Items.Add(panel);
        }
    }
}
