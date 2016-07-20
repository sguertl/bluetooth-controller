using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Android.Graphics.Drawables;

namespace BluetoothController
{
    [Activity(Label = "SearchedDevices")]
    public class SearchDevices : Activity, IEstablishConnection
    {
        // Members
        private BluetoothAdapter m_BtAdapter;
        private IntentFilter m_Filter; // Used to filter events when searching
        private MyBroadcastreciver m_Receiver;
        private Button m_BtSearch;
        private LinearLayout m_Linear;
        private ListView m_ListView;
        private List<string> m_Uuids; // List of UUIDs
        private GradientDrawable m_Drawable;
        private ProgressDialog m_ProgressDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchedLayout);

            Init();

            // Starting to discover for devices nearby
            OnSearch();
        }

        /// <summary>
        /// Initializes and modifies object
        /// </summary>
        public void Init()
        {
            // Initializing objects
            m_Uuids = new List<string>();
            m_ListView = FindViewById<ListView>(Resource.Id.listViewSearched);
            m_BtSearch = FindViewById<Button>(Resource.Id.btSearchDevices);
            m_Drawable = new GradientDrawable();
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear3);
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_Filter = new IntentFilter();
            m_Receiver = new MyBroadcastreciver(this);



            m_ProgressDialog = new ProgressDialog(this);
            m_ProgressDialog.SetMessage("Scanning for Devices...");
            m_ProgressDialog.SetCancelable(false);
            m_ProgressDialog.CancelEvent += delegate { m_ProgressDialog.Dismiss(); m_BtAdapter.CancelDiscovery(); };

            // Setting background color of ListView
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black);

            // Adding event when clicking on a ListViewItem 
            m_ListView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) => { OnItemClick(sender, e); };
       
            // Setting text color of button
            m_BtSearch.SetTextColor(Android.Graphics.Color.Black);

            // Setting border
            m_Drawable.SetShape(ShapeType.Rectangle);
            m_Drawable.SetStroke(2, Android.Graphics.Color.Black);
            m_Drawable.SetColor(Android.Graphics.Color.White);
            m_BtSearch.Background = m_Drawable;

            // Adding event when clicking the search button
            m_BtSearch.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                // Changing background if button was touched
                if (e2.Event.Action == MotionEventActions.Down)
                {
                    m_BtSearch.SetBackgroundColor(Android.Graphics.Color.Aquamarine);
                }
                // Starting to search when button was released
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    OnSearch();
                    m_BtSearch.Background = m_Drawable;
                }
            };

            // Setting activity background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);

            // Filtering events
            m_Filter.AddAction(BluetoothDevice.ActionFound);
            m_Filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
            m_Filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            m_Filter.AddAction(BluetoothDevice.ActionUuid);

            // Registering events and forwarding them to the broadcast object
            RegisterReceiver(m_Receiver, m_Filter);
        }

        /// <summary>
        /// Displays a toast
        /// </summary>
        /// <param name="message">Text to display</param>
        public void GiveAMessage(String message)
        {
            Toast.MakeText(ApplicationContext, message, 0).Show();
        }

        /// <summary>
        /// Starts to discover new devices nearby
        /// </summary>
        public void OnSearch()
        {
            // Removing old devices
            m_ListView.Adapter = null;
            m_Receiver.ResetList();
            m_ProgressDialog.Show();
            // Avoiding multiple searches
            m_BtAdapter.CancelDiscovery();
            m_BtAdapter.StartDiscovery();
        }

       
        /// <summary>
        /// Displays found devices
        /// </summary>
        /// <param name="l"></param>
        public void SetAdapterToListView(List<String> l)
        {
            ArrayAdapter<String> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, l);
            m_ListView.Adapter = adapter;
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Gray);
            m_ProgressDialog.Dismiss();
        }


        public void OnItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            // Displaying the chosen item on a TextView
            TextView view = (TextView)e.View;
            view.SetBackgroundColor(Android.Graphics.Color.Blue);
            String address = view.Text.Split('\n')[1];

            // Creating a BluetoothDevice object
            BluetoothDevice btDevice = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
            BuildConnection(btDevice, m_Uuids[e.Position]);
        }

        /// <summary>
        ///  Adds a UUID to the list
        /// </summary>
        /// <param name="uuid">UUID as String</param>
        public void AddUuid(String uuid)
        {
            m_Uuids.Add(uuid);
        }

        /// <summary>
        /// Builds a connection and starts a new activity
        /// </summary>
        /// <param name="bluetoothDevice"></param>
        /// <param name="uuid"></param>
        public void BuildConnection(BluetoothDevice bluetoothDevice, String uuid)
        {
            // Creating a ConnectionThread object
            ConnectedThread connect = new ConnectedThread(bluetoothDevice, uuid, this);
            connect.Start();

            // Starting new activity
            var activity2 = new Intent(this, typeof(ConnectedDevices));
            IList<String> ll = new List<string>();
            ll.Add(bluetoothDevice.Name);
            ll.Add(bluetoothDevice.Address);
            activity2.PutStringArrayListExtra("MyData", ll);
            StartActivity(activity2);        }
    }
}