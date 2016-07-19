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
    // ---------------------- DOKUMENTATION ----------------------------

    [Activity(Label = "PairedDevices")]
    public class PairedDevices : Activity, IEstablishConnection
    {
        // Member Variablen
        private ListView m_ListView;
        private BluetoothAdapter m_BtAdapter;
        private ICollection<BluetoothDevice> m_PairedDevice;
        private ArrayAdapter<String> m_Adapter;
        private LinearLayout m_Linear;
        private List<String> m_List;
        private List<String> m_UuidList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PairedLayout);
  
            Init();

            // Zeigt die bereits gekoppelten Devices an
            GetPairedDevices();
        }

        private void GetPairedDevices()
        {
            foreach (BluetoothDevice device in m_PairedDevice) { m_List.Add(device.Name + "\n" + device.Address);}  // Durchläuft die alle gekoppelten Devices und schreibt sie auf eine Liste
            m_Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, m_List);            // Wandelt die Liste in ein Adapter Array um
            m_ListView.Adapter = m_Adapter;                                                                         // Setzt den Adapter der ListView 
        }

        public void Init()
        {
            // Erzeugen der Objekte
            m_ListView = FindViewById<ListView>(Resource.Id.listView);
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear2);
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_PairedDevice = m_BtAdapter.BondedDevices;
            m_List = new List<String>();
            m_UuidList = new List<String>();

            // Activity Background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);   // Setzt die Background Color Weiß

            // Background Color [ListView]
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black); // Setzt die Background Color auf Schwarz
            m_ListView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) => { OnItemClick(sender, e); };
        }

        private void OnItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            TextView view = (TextView)e.View;
            string address = view.Text.Split('\n')[1];
            BluetoothDevice bluetoothDevice = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
            Console.WriteLine(bluetoothDevice.GetUuids()[0].Uuid.ToString());
            Console.WriteLine(bluetoothDevice.GetUuids()[0].Uuid);
            BuildConnection(bluetoothDevice, bluetoothDevice.GetUuids()[0].Uuid.ToString());
        }

        public void BuildConnection(BluetoothDevice bluetoothDevice, string uuid)
        {
            ConnectedThread connect = new ConnectedThread(bluetoothDevice, uuid);         // Erstellt ein Objekt von Connection Thread mit dem Bluetooth Device und mit der jeweiligen UUID
            connect.Start();                                                                      // Startet den Thread, um sich mit dem Device zu verbinden

            var activity2 = new Intent(this, typeof(ConnectedDevices));
            IList<String> ll = new List<string>();
            ll.Add(bluetoothDevice.Name);
            ll.Add(bluetoothDevice.Address);
            activity2.PutStringArrayListExtra("MyData", ll);
            StartActivity(activity2);
        }
    }
}