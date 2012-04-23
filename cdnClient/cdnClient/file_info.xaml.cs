using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using Microsoft.Phone.Tasks;
namespace cdnClient
{
    public partial class file_info : PhoneApplicationPage
    {
        public file_info()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string newparameter = this.NavigationContext.QueryString["parameter"];
            PageTitle.Text = newparameter;
            textRead.Text = "";
            String newparameter_des = newparameter + ".des";
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            try
            {
                // Specify the file path and options.
                using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\" + newparameter_des, FileMode.Open, myStore))
                {
                    // Read the data.
                    using (var isoFileReader = new StreamReader(isoFileStream))
                    {
                        string read_line = isoFileReader.ReadLine();
                        while (read_line != null)
                        {
                            textRead.Text = textRead.Text + "\n" + read_line;
                            read_line = isoFileReader.ReadLine();
                        }
                    }
                }

            }
            catch
            {
                // Handle the case when the user attempts to click the Read button first.
                textRead.Text = "Need to create directory and the file first.";
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            String parameter = PageTitle.Text;
            if (parameter.Contains(".txt"))
            {
                NavigationService.Navigate(new Uri(string.Format("/text.xaml?parameter={0}", parameter), UriKind.Relative));
            }
            else
            {
                if (parameter.Contains(".wmv"))
                {
                    MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();
                    mediaPlayerLauncher.Media = new Uri("MyFolder\\" + parameter, UriKind.Relative);
                    //replace "gags" with your file path.
                    mediaPlayerLauncher.Location = MediaLocationType.Data;
                    mediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop | MediaPlaybackControls.All;
                    mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;
                    mediaPlayerLauncher.Show();
                }
                else
                {
                    NavigationService.Navigate(new Uri(string.Format("/video_image.xaml?parameter={0}", parameter), UriKind.Relative));
                }
            }
        }

        private void button2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            String parameter = PageTitle.Text;
            NavigationService.Navigate(new Uri(string.Format("/add_comment.xaml?parameter={0}", parameter), UriKind.Relative));
            
        }

        private void button3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            String parameter = PageTitle.Text;
            NavigationService.Navigate(new Uri(string.Format("/add_comment_new.xaml?parameter={0}", parameter), UriKind.Relative));


        }
    }
}