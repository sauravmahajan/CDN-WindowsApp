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
    public partial class resume_download : PhoneApplicationPage
    {
        public resume_download()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string newparameter = this.NavigationContext.QueryString["parameter"];
            PageTitle.Text = newparameter;
            textRead.Text = "";
            textBlock1.Text = GlobalVar.doTransfer.ToString();
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
            //textBlock1.Text = " Resume Available = " + GlobalVar.resume.ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dthread d1 = new Dthread(PageTitle.Text);
            Thread oThread = new Thread(new ThreadStart(d1.download));
            GlobalVar.item = PageTitle.Text;
            GlobalVar.iscomplete = false;
            oThread.Start();
            

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