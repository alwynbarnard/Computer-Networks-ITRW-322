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
    /// Interaction logic for CallsList.xaml
    /// </summary>
    /// 
    

    public partial class CallsList : Page
    {
        CallsItem[] item = new CallsItem[10];
        Uri[] uris = new Uri[6];
        BitmapImage[] images = new BitmapImage[6];

        public CallsList()
        {
            InitializeComponent();
            FillItem();

            try
            {
                Add_Pics_To_List();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error adding pics to list", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            for (int i = 0; i < 50; i++)
            {
                AddItem(i % 5);
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
        public void AddItem(int index)
        {
            Button test = new Button();

            test.Background = new ImageBrush(images[index]);
            test.FontStretch = FontStretches.Condensed;
            test.Height = 50;
            test.Width = 50;

            ListBox1.Items.Add(test);
        }

        //Initialize all CallsItems
        public void FillItem()
        {
            for (int i = 0; i < item.Length; i++)
            {
                item[i] = new CallsItem();
            }
        }
    }
}
