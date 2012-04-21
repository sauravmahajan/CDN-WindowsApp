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
using Microsoft.Phone.Net.NetworkInformation;

namespace cdnClient
{
    public enum Networks{Wifi, DataConnection, Roaming}
    
    public partial class configuration : PhoneApplicationPage
    {
        public static bool[] NetworksAllowed = new bool[3];
        public configuration()
        {
            
            InitializeComponent();

            //read from config file
            NetworksAllowed[(int)Networks.Wifi] = true;
            NetworksAllowed[(int)Networks.DataConnection] = false;
            NetworksAllowed[(int)Networks.Roaming] = false;

            DeviceNetworkInformation.NetworkAvailabilityChanged += new EventHandler<NetworkNotificationEventArgs>(runOppurtunistic);
            //Microsoft.Phone.Net.NetworkInformation.      
        }

        public static void runOppurtunistic(object sender, NetworkNotificationEventArgs e){
           
            String result = "";

            result += "network connected: " +e.NetworkInterface.InterfaceType.ToString()+ "\n";
            result += "network connected name: " + e.NetworkInterface.InterfaceName.ToString() + "\n";

            switch (e.NotificationType)
            {
                case NetworkNotificationType.InterfaceConnected: case NetworkNotificationType.CharacteristicUpdate:
                    GlobalVar.doTransfer = false;
                    if (DeviceNetworkInformation.IsCellularDataEnabled && NetworksAllowed[(int)Networks.DataConnection] && e.NetworkInterface.Characteristics != NetworkCharacteristics.Roaming)
                    {
                        GlobalVar.doTransfer = true;
                    }
                    if (DeviceNetworkInformation.IsWiFiEnabled && NetworksAllowed[(int)Networks.Wifi])
                    {
                        GlobalVar.doTransfer = true;
                    }
                    if (DeviceNetworkInformation.IsCellularDataEnabled && NetworksAllowed[(int)Networks.Roaming] && e.NetworkInterface.Characteristics == NetworkCharacteristics.Roaming )
                    {
                        GlobalVar.doTransfer = true;
                    }
                    break;

               default :
                    //do nothing must be other events
                    break;
            }

        }
    }
}