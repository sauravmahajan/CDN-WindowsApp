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
    public partial class add_comment_new : PhoneApplicationPage
    {
        public add_comment_new()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string newparameter = this.NavigationContext.QueryString["parameter"];
            PageTitle.Text = newparameter;
        
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            // Create a new folder and call it "MyFolder".
            //myStore.CreateDirectory("MyFolder");
            //string temp = GlobalVar.client.Receive();
            String name = PageTitle.Text + ".comment";
            // Specify the file path and options.
            string sMSG = textBox1.Text;
            StreamWriter sw = new StreamWriter(new IsolatedStorageFileStream("MyFolder\\" + name, FileMode.Append, myStore));
            sw.WriteLine(sMSG); //Wrting to the file
            sw.Close();
        }
    }
}