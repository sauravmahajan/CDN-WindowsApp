﻿#pragma checksum "C:\Users\saurav\CDN-WindowsApp\cdnClient\cdnClient\explorer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F69332800050B87F6E83321A1356D253"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace cdnClient {
    
    
    public partial class explorer : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock textBlock2;
        
        internal System.Windows.Controls.ListBox DoneItemsListBox;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.ListBox toDoItemsListBox;
        
        internal System.Windows.Controls.TextBox newToDoTextBox;
        
        internal System.Windows.Controls.Button newToDoAddButton;
        
        internal System.Windows.Controls.Button refresh;
        
        internal System.Windows.Controls.Button button3;
        
        internal System.Windows.Controls.TextBox RemoteUpload;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/cdnClient;component/explorer.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.textBlock2 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock2")));
            this.DoneItemsListBox = ((System.Windows.Controls.ListBox)(this.FindName("DoneItemsListBox")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.toDoItemsListBox = ((System.Windows.Controls.ListBox)(this.FindName("toDoItemsListBox")));
            this.newToDoTextBox = ((System.Windows.Controls.TextBox)(this.FindName("newToDoTextBox")));
            this.newToDoAddButton = ((System.Windows.Controls.Button)(this.FindName("newToDoAddButton")));
            this.refresh = ((System.Windows.Controls.Button)(this.FindName("refresh")));
            this.button3 = ((System.Windows.Controls.Button)(this.FindName("button3")));
            this.RemoteUpload = ((System.Windows.Controls.TextBox)(this.FindName("RemoteUpload")));
        }
    }
}

