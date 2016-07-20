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

namespace BluetoothController
{
    [Activity(Label = "PairedDevices", Theme = "@android:style/Theme.Light.NoTitleBar")]
    public class PairedDevices : Activity, IEstablishConnection
    {
        // Members
        private ListView m_ListView;
        private BluetoothAdapter m_BtAdapter;
        private ICollection<BluetoothDevice> m_PairedDevice;
        private ArrayAdapter<String> m_Adapter;
        private LinearLayout m_Linear;
        private List<String> m_List;
        private List<String> m_UuidList;
        private bool m_IsConnected;


        public bool IsConnected
        {
            get { return m_IsConnected; }
            set { m_IsConnected = value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PairedLayout);
  
            Init();

            // Displaying the paired devices
            GetPairedDevices();
        }


        private void GetPairedDevices()
        {
            // Displaying all paired devices on a ListView
            foreach (BluetoothDevice device in m_PairedDevice) { m_List.Add(device.Name + "\n" + device.Address);}
            m_Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, m_List);
            m_ListView.Adapter = m_Adapter;
        }

        /// <summary>
        /// Initializes and modifies objects
        /// </summary>
        public void Init()
        {
            // Initializing objects
            m_ListView = FindViewById<ListView>(Resource.Id.listView);
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear2);
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_PairedDevice = m_BtAdapter.BondedDevices;
            m_List = new List<String>();
            m_UuidList = new List<String>();
            m_IsConnected = true;

            // Setting activity background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);

            // Setting background color of the ListView
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black);

            // Adding handler when clicking on a ListViewItem
            m_ListView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) => { OnItemClick(sender, e); };
        }

        private void OnItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            TextView view = (TextView)e.View;
            string address = view.Text.Split('\n')[1];
            BluetoothDevice bluetoothDevice = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
            BuildConnection(bluetoothDevice, bluetoothDevice.GetUuids()[0].Uuid.ToString());
        }

        /// <summary>
        /// Builds a connection to a BluetoothDevice
        /// </summary>
        /// <param name="bluetoothDevice"></param>
        /// <param name="uuid"></param>
        public void BuildConnection(BluetoothDevice bluetoothDevice, string uuid)
        {
            // Creating a ConnectionThread object
            ConnectedThread connect = new ConnectedThread(bluetoothDevice, uuid, this);
            connect.Start();
        }

        public void StartActivity(BluetoothDevice bluetoothDevice)
        {
            var activity2 = new Intent(this, typeof(ConnectedDevices));
            IList<String> ll = new List<string>();
            ll.Add(bluetoothDevice.Name);
            ll.Add(bluetoothDevice.Address);
            activity2.PutStringArrayListExtra("MyData", ll);
            StartActivity(activity2);
        }
    }
}