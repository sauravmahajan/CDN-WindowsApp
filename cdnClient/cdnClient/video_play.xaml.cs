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
using Microsoft.Phone.Tasks;

namespace cdnClient
{
    public partial class video_play : PhoneApplicationPage
    {
        String PageTitle;
        MediaPlayerLauncher mediaPlayerLauncher;
        public video_play()
        {
            InitializeComponent();
            mediaPlayerLauncher= new MediaPlayerLauncher();
            
            //replace "gags" with your file path.
            mediaPlayerLauncher.Location = MediaLocationType.Data;
            mediaPlayerLauncher.Controls = MediaPlaybackControls.All;
            mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;
            
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string newparameter = this.NavigationContext.QueryString["parameter"];
            PageTitle = newparameter;
            mediaPlayerLauncher.Media = new Uri("MyFolder\\" + PageTitle, UriKind.Relative);
            mediaPlayerLauncher.Show();

        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                mediaplayer.Source = new Uri("MyFolder\\" + PageTitle, UriKind.Relative);
                //mediaplayer
                mediaplayer.Play();
            }
            catch(Exception ex){
            }
            
        }
    }
}