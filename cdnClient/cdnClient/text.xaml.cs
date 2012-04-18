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

namespace cdnClient
{
    public partial class text : PhoneApplicationPage
    {
        public text()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string newparameter = this.NavigationContext.QueryString["parameter"];
            PageTitle.Text = newparameter;

            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            try
            {
                // Specify the file path and options.
                using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\"+newparameter, FileMode.Open, myStore))
                {
                    // Read the data.
                    using (var isoFileReader = new StreamReader(isoFileStream))
                    {
                        string read_line = isoFileReader.ReadLine();
                        while (read_line != null)
                        {
                            textRead.Text = textRead.Text +"\n"+ read_line;
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
    }
}