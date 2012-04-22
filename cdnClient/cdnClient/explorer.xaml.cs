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
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace cdnClient
{
    
    public partial class explorer: PhoneApplicationPage, INotifyPropertyChanged
    {
        // Data context for the local database
        public static ToDoDataContext toDoDB;

        // Define an observable collection property that controls can bind to.
        private ObservableCollection<ToDoItem> _toReallyDoItems;
        public ObservableCollection<ToDoItem> ToReallyDoItems
        {
            get
            {
                return _toReallyDoItems;
            }
            set
            {
                if (_toReallyDoItems != value)
                {
                    _toReallyDoItems = value;
                    NotifyPropertyChanged("ToReallyDoItems");
                }
            }
        }
        private ObservableCollection<ToDoItem> _toDoItems;
        public ObservableCollection<ToDoItem> ToDoItems
        {
            get
            {
                return _toDoItems;
            }
            set
            {
                if (_toDoItems != value)
                {
                    _toDoItems = value;
                    NotifyPropertyChanged("ToDoItems");
                }
            }
        }

        // Constructor
        public explorer()
        {
            InitializeComponent();
            // Connect to the database and instantiate data context.
            toDoDB = new ToDoDataContext(ToDoDataContext.DBConnectionString);

            // Data context and observable collection are children of the main page.
            this.DataContext = this;
        }
        
        private void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {   
            // Cast parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                ToDoItem toDoForDelete = button.DataContext as ToDoItem;

                // Remove the to-do item from the observable collection.
                ToDoItems.Remove(toDoForDelete);

                // Remove the to-do item from the local database.
                toDoDB.ToDoItems.DeleteOnSubmit(toDoForDelete);

                // Save changes to the database.
                toDoDB.SubmitChanges();

                // Put the focus back to the main page.
                this.Focus();
            }
        }

       private void openTaskButton_Click1(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext as ToDoItem ;

            String parameter = item.ItemName;//"kimi.jpg";
            NavigationService.Navigate(new Uri(string.Format("/image_page.xaml?parameter={0}", parameter), UriKind.Relative));
        
        }

        private void openTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext as ToDoItem;

            String parameter = item.ItemName;//"kimi.jpg";
            NavigationService.Navigate(new Uri(string.Format("/file_info.xaml?parameter={0}", parameter), UriKind.Relative));
            
            /*if (parameter.Contains(".txt"))
            {
                NavigationService.Navigate(new Uri(string.Format("/text.xaml?parameter={0}", parameter), UriKind.Relative));
            }
            else
            {
                if (parameter.Contains(".wmv"))
                {
                    MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();
                    mediaPlayerLauncher.Media = new Uri("MyFolder\\" + parameter, UriKind.Relative);
                    //replace "gags" with your file path.
                    mediaPlayerLauncher.Location = MediaLocationType.Data;
                    mediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop | MediaPlaybackControls.All;
                    mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;
                    mediaPlayerLauncher.Show();
                }
                else
                {
                    NavigationService.Navigate(new Uri(string.Format("/video_image.xaml?parameter={0}", parameter), UriKind.Relative));
                }
            }*/
        }

        private void newToDoTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear the text box when it gets focus.
            newToDoTextBox.Text = String.Empty;
        }
        private void download(String name)
        {
            GlobalVar.client.Send("1" + "\n");
            GlobalVar.client.Send(name + "\n");
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            // Create a new folder and call it "MyFolder".
            myStore.CreateDirectory("MyFolder");
            string temp = GlobalVar.client.Receive();
            // Specify the file path and options.
            using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\" + name, FileMode.OpenOrCreate, myStore))
            {
                //Write the data
                using (var isoFileWriter = new StreamWriter(isoFileStream))
                {
                    bool done = false;
                    while (temp.CompareTo("null") != 0)
                    {
                        string[] all = temp.Split('\n');
                        for (int i = 0; i < all.Length; i++)
                        {
                            if (all[i].CompareTo("null") != 0)
                            {
                                if (all[i].CompareTo("") != 0)
                                    isoFileStream.WriteByte(Convert.ToByte(all[i]));
                            }
                            else
                            {
                                done = true;
                                break;
                            }
                        }
                        if (done) break;

                        temp = GlobalVar.client.Receive();
                    }
                }
            }
        }
        private void newToDoAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new to-do item based on the text box.
            ToDoItem newToDo = new ToDoItem { ItemName = newToDoTextBox.Text };

            // Add a to-do item to the observable collection.
            ToReallyDoItems.Add(newToDo);

            // Add a to-do item to the local database.
            toDoDB.ToDoItems.InsertOnSubmit(newToDo);

            toDoDB.SubmitChanges();

            Dthread d1 = new Dthread(newToDoTextBox.Text);
            Thread oThread = new Thread(new ThreadStart(d1.download));

            //TODO: 
            download(newToDoTextBox.Text+".des");
            download(newToDoTextBox.Text + ".comment");
            // Start the thread
            oThread.Start();

            GlobalVar.item = newToDoTextBox.Text;
            GlobalVar.iscomplete = false;
            //update(newToDoTextBox.Text);
            //download(newToDoTextBox.Text);
        }

        //static ToDoItem item;
        public void update(String name)
        {
           /* GlobalVar.item = (from ToDoItem todo in toDoDB.ToDoItems
                        where todo.ItemName == name
                        select todo).Single();*/
            //item.IsComplete = true;
            //toDoDB.SubmitChanges();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);

            // Save changes to the database.
            toDoDB.SubmitChanges();
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Define the query to gather all of the to-do items.
            var toDoItemsInDB = from ToDoItem todo in toDoDB.ToDoItems where todo.IsComplete == true
                                select todo;

            // Execute the query and place the results into a collection.
            ToDoItems = new ObservableCollection<ToDoItem>(toDoItemsInDB);

            // Define the query to gather all of the to-do items.
            var toReallyDoItemsInDB = from ToDoItem todo in toDoDB.ToDoItems
                                where todo.IsComplete == false
                                select todo;

            // Execute the query and place the results into a collection.
            ToReallyDoItems = new ObservableCollection<ToDoItem>(toReallyDoItemsInDB);
            // Call the base method.
            base.OnNavigatedTo(e);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            //toDoDB.ToDoItems.InsertOnSubmit(GlobalVar.item);
            var itemt =(from ToDoItem todo in toDoDB.ToDoItems
                              where todo.ItemName == GlobalVar.item
                              select todo);
            if (itemt.Count() > 0)
            {
                var item = itemt.Single();
                item.IsComplete = GlobalVar.iscomplete;
            }
            toDoDB.SubmitChanges();
            //toDoDB.SubmitChanges();
            // Define the query to gather all of the to-do items.
            var toDoItemsInDB = from ToDoItem todo in toDoDB.ToDoItems
                                where todo.IsComplete == true
                                select todo;

            // Execute the query and place the results into a collection.
            ToDoItems = new ObservableCollection<ToDoItem>(toDoItemsInDB);

            // Define the query to gather all of the to-do items.
            var toReallyDoItemsInDB = from ToDoItem todo in toDoDB.ToDoItems
                                      where todo.IsComplete == false
                                      select todo;

            // Execute the query and place the results into a collection.
            ToReallyDoItems = new ObservableCollection<ToDoItem>(toReallyDoItemsInDB);

        }

        private void button3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Obtain a virtual store for the application.
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            try
            {
                // Specify the file path and options.
                GlobalVar.client.Send("" + 2 + "\n");
                GlobalVar.client.Send(RemoteUpload.Text + "\n");
                using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\" + RemoteUpload.Text, FileMode.Open, myStore))
                {
                    // Read the data.
                    using (var isoFileReader = new StreamReader(isoFileStream))
                    {
                        int temp_send = isoFileStream.ReadByte();
                        while (temp_send != -1)
                        {
                            GlobalVar.client.Send(temp_send + "\n");
                            temp_send = isoFileStream.ReadByte();
                        }
                        GlobalVar.client.Send("null" + "\n");
                    }
                }
                // GlobalVar.client.Send("-1"+ "\n");

            }
            catch
            {
                // Handle the case when the user attempts to click the Read button first.
                //txtRead.Text = "Need to create directory and the file first.";
            }
        }

        private void checkStateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext as ToDoItem;

            String parameter = item.ItemName;//"kimi.jpg";
            NavigationService.Navigate(new Uri(string.Format("/resume_download.xaml?parameter={0}", parameter), UriKind.Relative));
        }
    }
    public class ToDoDataContext : DataContext
    {
        // Specify the connection string as a static, used in main page and app.xaml.
        public static string DBConnectionString = "Data Source=isostore:/ToDo.sdf";

        // Pass the connection string to the base class.
        public ToDoDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a single table for the to-do items.
        public Table<ToDoItem> ToDoItems;
    }

    [Table]
    public class ToDoItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _toDoItemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ToDoItemId
        {
            get
            {
                return _toDoItemId;
            }
            set
            {
                if (_toDoItemId != value)
                {
                    NotifyPropertyChanging("ToDoItemId");
                    _toDoItemId = value;
                    NotifyPropertyChanged("ToDoItemId");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _itemName;

        [Column]
        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                if (_itemName != value)
                {
                    NotifyPropertyChanging("ItemName");
                    _itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }

        private string _itemDes;

        [Column]
        public string ItemDes
        {
            get
            {
                return _itemDes;
            }
            set
            {
                if (_itemDes != value)
                {
                    NotifyPropertyChanging("ItemDes");
                    _itemDes = value;
                    NotifyPropertyChanged("ItemDes");
                }
            }
        }

        // Define completion value: private field, public property and database column.
        public bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get
            {
                return _isComplete;
            }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        public void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class Dthread
    {
        //public ToDoDataContext toDoDB2 = ;
        public String name;
        public Dthread(String x)
        {
            name = x;
        }
        
        public void download()
        {
            if (!GlobalVar.doTransfer)
            {
                GlobalVar.client.Send("1" + "\n");
                GlobalVar.client.Send(name + "\n");
                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

                // Create a new folder and call it "MyFolder".
                myStore.CreateDirectory("MyFolder");
                using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\" + name, FileMode.OpenOrCreate, myStore))
                {
                    try
                    {
                        string temp = GlobalVar.client.Receive();
                        // Specify the file path and options.

                        //Write the data
                        using (var isoFileWriter = new StreamWriter(isoFileStream))
                        {
                            bool done = false;
                            while (temp.CompareTo("null") != 0)
                            {
                                string[] all = temp.Split('\n');
                                for (int i = 0; i < all.Length; i++)
                                {
                                    if (all[i].CompareTo("null") != 0)
                                    {
                                        if (all[i].CompareTo("") != 0)
                                            isoFileStream.WriteByte(Convert.ToByte(all[i]));
                                    }
                                    else
                                    {
                                        done = true;
                                        break;
                                    }
                                }
                                if (done) break;

                                temp = GlobalVar.client.Receive();
                            }

                            GlobalVar.iscomplete = true;
                            GlobalVar.resume = false;
                        }
                    }
                    catch (Exception e)
                    {
                        GlobalVar.resume = true;
                    }
                }

                //update();
                
                //MessageBox.Show("File Downloaded: "+name);
            }
            else {
                GlobalVar.resume = true;
            }

        }

        public void resume()
        {
            if (!GlobalVar.doTransfer)
            {

                GlobalVar.client.Send("3" + "\n");
                GlobalVar.client.Send(name + "\n");
                IsolatedStorageFile myStore2 = IsolatedStorageFile.GetUserStoreForApplication();

                // Create a new folder and call it "MyFolder".
                myStore2.CreateDirectory("MyFolder");
                using (var isoFileStream2 = new IsolatedStorageFileStream("MyFolder\\" + name, FileMode.OpenOrCreate, myStore2))
                {
                    String tosendint = isoFileStream2.Length.ToString();
                    GlobalVar.client.Send(tosendint + "\n");
                    Thread.SpinWait(1000);
                    Thread.Sleep(1000);
                    isoFileStream2.Close();
                }
                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

                // Create a new folder and call it "MyFolder".
                myStore.CreateDirectory("MyFolder");
                using (var isoFileStream = new IsolatedStorageFileStream("MyFolder\\" + name, FileMode.OpenOrCreate, myStore))
                {
                    
                    try
                    {
                        String wtf = "";
                        string temp = GlobalVar.client.Receive();
                        isoFileStream.Position = isoFileStream.Length;
                        // Specify the file path and options.

                        //Write the data
                        using (var isoFileWriter = new StreamWriter(isoFileStream))
                        {
                            bool done = false;
                            while (temp.CompareTo("null") != 0)
                            {
                                string[] all = temp.Split('\n');
                                for (int i = 0; i < all.Length; i++)
                                {
                                    if (all[i].CompareTo("null") != 0)
                                    {
                                        if (all[i].CompareTo("") != 0)
                                            isoFileStream.WriteByte(Convert.ToByte(all[i]));
                                    }
                                    else
                                    {
                                        done = true;
                                        break;
                                    }
                                }
                                if (done) break;

                                temp = GlobalVar.client.Receive();
                            }
                        }

                        GlobalVar.iscomplete = true;
                        GlobalVar.resume = false;
                    }
                    catch (Exception e)
                    {
                        String fuck = "something went very wrong";
                    
                    }

                }
            }
        }
    };
}