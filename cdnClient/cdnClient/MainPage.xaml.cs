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
//using System.Windows.Controls;

namespace cdnClient
{

    public static class GlobalVar
    {
        public static SocketClient client;
        public static string temp;
        public static String item;
        public static bool iscomplete;
    }
    public partial class MainPage : PhoneApplicationPage
    {
        //SocketClient client_temp = new SocketClient();
        // Constants
        const int ECHO_PORT = 7;  // The Echo protocol uses port 7 in this sample
        const int QOTD_PORT = 17; // The Quote of the Day (QOTD) protocol uses port 17 in this sample

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle the btnEcho_Click event by sending text to the echo server and outputting the response
        /// </summary>
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            // Clear the log 
            ClearLog();

            // Make sure we can perform this action with valid data
            if (ValidateRemoteHost() && ValidateInput() && ValidateUsername() && Validatepassword() )
            {
                // Instantiate the SocketClient
                GlobalVar.client = new SocketClient();

                // Attempt to connect to the echo server
                Log(String.Format("Connecting to server '{0}' over port {1} ...", txtRemoteHost.Text, Convert.ToInt32(txtPortNo.Text)), true);
                string result = GlobalVar.client.Connect(txtRemoteHost.Text,Convert.ToInt32(txtPortNo.Text));
                Log(result, false);
                //GlobalVar.client = client;
                // Attempt to send our message to be echoed to the echo server
                //Log(String.Format("Sending '{0}' to server ...", txtInput.Text), true);
                /*ValidateUsername();
                Validatepassword();
                    result = client.Send(""+txtPassword.Text);
                    Log(result,false);
                    result = client.Send("" + txtUsername.Text);
                    Log(result, false);*/
                
                result = GlobalVar.client.Send(txtUsername.Text+"\n");
                Log(result, false);
                result = GlobalVar.client.Send(txtPassword.Text + "\n");
                Log(result, false);
                //System.Threading.Thread.Sleep(20000);

                // Receive a response from the echo server
                Log("Requesting Receive ...", true);
                result = GlobalVar.client.Receive();
                if (result.CompareTo("done")==0) {

                    NavigationService.Navigate(new Uri("/explorer.xaml", UriKind.Relative));
                }
                Log(result, false);

                // Close the socket connection explicitly
                //GlobalVar.client.Close();
            }

        }

        #region UI Validation
        /// <summary>
        /// Validates the txtInput TextBox
        /// </summary>
        /// <returns>True if the txtInput TextBox contains valid data, False otherwise</returns>
        private bool ValidateInput()
        {
            // txtInput must contain some text
            if (String.IsNullOrWhiteSpace(txtPortNo.Text))
            {
                MessageBox.Show("Please enter a Port No");
                txtPortNo.Text = "5010";
                return false;
            }

            return true;
        }


        private bool ValidateUsername()
        {
            // txtInput must contain some text
            if (String.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter a username");
                txtUsername.Text = "user";
                return false;
            }

            return true;
        }

        private bool Validatepassword()
        {
            // txtInput must contain some text
            if (String.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter a Password");
                txtPassword.Text = "userpasswd";
                return false;
            }

            return true;
        }
        /// <summary>
        /// Validates the txtRemoteHost TextBox
        /// </summary>
        /// <returns>True if the txtRemoteHost contains valid data, False otherwise</returns>
        private bool ValidateRemoteHost()
        {
            // The txtRemoteHost must contain some text
            if (String.IsNullOrWhiteSpace(txtRemoteHost.Text))
            {
                MessageBox.Show("Please enter a host name");
                //txtRemoteHost.Text = "10.22.254.228";
                //txtRemoteHost.Text = "144.16.141.190";
                //txtRemoteHost.Text = "10.148.0.153";
                //txtRemoteHost.Text = "10.193.1.155";
                txtRemoteHost.Text = "192.168.43.147";
                return false;
            }

            return true;
        }
        #endregion

        #region Logging
        /// <summary>
        /// Log text to the txtOutput TextBox
        /// </summary>
        /// <param name="message">The message to write to the txtOutput TextBox</param>
        /// <param name="isOutgoing">True if the message is an outgoing (client to server) message, False otherwise</param>
        /// <remarks>We differentiate between a message from the client and server 
        /// by prepending each line  with ">>" and "<<" respectively.</remarks>
        private void Log(string message, bool isOutgoing)
        {
            string direction = (isOutgoing) ? ">> " : "<< ";
            txtOutput.Text += Environment.NewLine + direction + message;
        }

        /// <summary>
        /// Clears the txtOutput TextBox
        /// </summary>
        private void ClearLog()
        {
            txtOutput.Text = String.Empty;
        }
        #endregion

        private void txtOutput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

       
    }
}