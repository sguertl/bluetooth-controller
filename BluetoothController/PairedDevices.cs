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
            foreach (BluetoothDevice device in m_PairedDevice) { m_List.Add(device.Name + "\n" + device.Address);}  // Durchl‰uft die alle gekoppelten Devices und schreibt sie auf eine Liste
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

            // Activity Background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);   // Setzt die Background Color Weiﬂ

            // Background Color [ListView]
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black); // Setzt die Background Color auf Schwarz
        }

        public void BuildConnection()
        {
            
        }
    }
}