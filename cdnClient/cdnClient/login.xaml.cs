using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
namespace cdnClient
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/sending.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.client.Send("1" + "\n");
            GlobalVar.client.Send(textDownload.Text + "\n");
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            // Create a new folder and call it "MyFolder".
            myStore.CreateDirectory("MyFolder");
            string temp = GlobalVar.client.Receive();
            // Specify the file path and options.
            using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\"+textDownload.Text, FileMode.OpenOrCreate, myStore))
            {
                //Write the data
                using (var isoFileWriter = new StreamWriter(isoFileStream))
                {

                   /* bool done = false;
                    while (temp.CompareTo("null")!=0)
                    {
                        string[] all = temp.Split('\n');
                        for (int i = 0; i < all.Length; i++)
                        {
                            if (all[i].CompareTo("null") != 0)
                            {
                                isoFileWriter.WriteLine(all[i]);
                            }
                            else
                            {
                                done = true;
                                break;
                            }
                        }
                        if (done) break;

                        temp = GlobalVar.client.Receive();
                    }*/
                    bool done = false;
                    while (temp.CompareTo("null") != 0)
                    {
                        string[] all = temp.Split('\n');
                        for (int i = 0; i < all.Length; i++)
                        {
                            if (all[i].CompareTo("null") != 0)
                            {
                                if (all[i].CompareTo("") != 0)
                                isoFileStream.WriteByte(Convert.ToByte(all[i]));
                            }
                            else
                            {
                                done = true;
                                break;
                            }
                        }
                        if (done) break;

                        temp = GlobalVar.client.Receive();
                    }
                }
            }

            // Obtain the virtual store for the application.
            

            
                    
            NavigationService.Navigate(new Uri("/downloading.xaml", UriKind.Relative));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            // Obtain a virtual store for the application.
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            try
            {
            // Specify the file path and options.
                GlobalVar.client.Send(""+2+"\n");
                GlobalVar.client.Send(RemoteUpload.Text + "\n");
                using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\"+RemoteUpload.Text, FileMode.Open, myStore))
            {
                // Read the data.
                using (var isoFileReader = new StreamReader(isoFileStream))
                {
                    int temp_send=isoFileStream.ReadByte();
                    while (temp_send !=-1)
                    {
                        GlobalVar.client.Send(temp_send+"\n");
                        temp_send = isoFileStream.ReadByte();
                    }
                    GlobalVar.client.Send("null"+"\n");
                }
            }
               // GlobalVar.client.Send("-1"+ "\n");

        }
        catch
        {
            // Handle the case when the user attempts to click the Read button first.
            //txtRead.Text = "Need to create directory and the file first.";
        }
      }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayerLauncher objMPlayerLauncher = new MediaPlayerLauncher();
            objMPlayerLauncher.Media = new Uri("slap.wmv", UriKind.Relative);
            //replace "gags" with your file path.
            objMPlayerLauncher.Location = MediaLocationType.Data;
            objMPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop;
            objMPlayerLauncher.Show();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            // The image will be read from isolated storage into the following byte array

            byte[] data;



            // Read the entire image in one go into a byte array

            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                // Open the file - error handling omitted for brevity

                // Note: If the image does not exist in isolated storage the following exception will be generated:

                // System.IO.IsolatedStorage.IsolatedStorageException was unhandled

                // Message=Operation not permitted on IsolatedStorageFileStream

                using (IsolatedStorageFileStream isfs = isf.OpenFile("MyFolder\\kimi.jpg", FileMode.Open, FileAccess.Read))
                {

                    // Allocate an array large enough for the entire file

                    data = new byte[isfs.Length];



                    // Read the entire file and then close it

                    isfs.Read(data, 0, data.Length);

                    isfs.Close();

                }

            }



            // Create memory stream and bitmap

            MemoryStream ms = new MemoryStream(data);

            BitmapImage bi = new BitmapImage();



            // Set bitmap source to memory stream

            bi.SetSource(ms);



            // Create an image UI element – Note: this could be declared in the XAML instead

            Image image = new Image();



            // Set size of image to bitmap size for this demonstration

            image.Height = bi.PixelHeight;

            image.Width = bi.PixelWidth;



            // Assign the bitmap image to the image’s source

            image.Source = bi;



            // Add the image to the grid in order to display the bit map

            ContentPanel.Children.Add(image);
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/explorer.xaml", UriKind.Relative));
        }
    }
}