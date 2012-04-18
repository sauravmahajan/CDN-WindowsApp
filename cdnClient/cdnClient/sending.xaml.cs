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
    public partial class Page2 : PhoneApplicationPage
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void hyperlinkButton1_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
                //SocketClient client = new SocketClient();

                // Attempt to connect to the echo server
                //string result = client.Connect("10.22.254.228", Convert.ToInt32("5010"));
                //string result = client.Connect("144.16.141.190", Convert.ToInt32("5010"));           
                GlobalVar.client.Send("" + 0 + "\n");
                String result = GlobalVar.client.Send(txtPassword.Text+"\n");
                GlobalVar.client.Send("-1"+"\n");
                
                //result = GlobalVar.client.Receive();
               

                // Close the socket connection explicitly
                //GlobalVar.client.Close();

        }

    }
}