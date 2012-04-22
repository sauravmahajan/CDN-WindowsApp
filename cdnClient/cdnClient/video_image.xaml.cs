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
using System.Windows.Media.Imaging;

namespace cdnClient
{
    public partial class video_image : PhoneApplicationPage
    {
        public video_image()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            string newparameter = this.NavigationContext.QueryString["parameter"];
            PageTitle.Text = newparameter;
            if (newparameter.Contains("wmv"))
            {
                MediaPlayerLauncher objMPlayerLauncher = new MediaPlayerLauncher();
                objMPlayerLauncher.Media = new Uri("MyFolder\\" + newparameter, UriKind.Relative);
                //replace "gags" with your file path.
                objMPlayerLauncher.Location = MediaLocationType.Data;
                objMPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop;
                objMPlayerLauncher.Show();
            }
            else
            {
                // Read the entire image in one go into a byte array
                byte[] data;
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {

                    // Open the file - error handling omitted for brevity

                    // Note: If the image does not exist in isolated storage the following exception will be generated:

                    // System.IO.IsolatedStorage.IsolatedStorageException was unhandled

                    // Message=Operation not permitted on IsolatedStorageFileStream
                    //string newparameter = this.NavigationContext.QueryString["parameter"];

                    using (IsolatedStorageFileStream isfs = isf.OpenFile("MyFolder\\" + newparameter, FileMode.Open, FileAccess.Read))
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

                image.Height = System.Windows.Application.Current.Host.Content.ActualHeight;

                image.Width = System.Windows.Application.Current.Host.Content.ActualWidth;



                // Assign the bitmap image to the image’s source

                image.Source = bi;



                // Add the image to the grid in order to display the bit map

                ContentPanel.Children.Add(image);
            }

        }
    }
}