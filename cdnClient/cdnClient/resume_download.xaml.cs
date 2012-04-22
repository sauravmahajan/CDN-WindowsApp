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
using System.Threading;

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
            textBlock1.Text = " Resume Available = " + GlobalVar.resume.ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dthread d1 = new Dthread(PageTitle.Text);
            Thread oThread = new Thread(new ThreadStart(d1.resume));
            oThread.Start();
            GlobalVar.item = PageTitle.Text;
            GlobalVar.iscomplete = false;

        }
    }
}