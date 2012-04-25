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

namespace cdnClient
{
    public partial class recorded_video_name : PhoneApplicationPage
    {
        public recorded_video_name()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            String parameter = textBox1.Text;
            NavigationService.Navigate(new Uri(string.Format("/video_record.xaml?parameter={0}", parameter), UriKind.Relative));
        }
    }
}