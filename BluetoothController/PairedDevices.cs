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
    public class PairedDevices : Activity
    {

        private ListView m_ListView;
        private BluetoothAdapter m_BtAdapter;
        private ICollection<BluetoothDevice> m_PairedDevice;
        private ArrayAdapter<String> m_Adapter;
        private LinearLayout m_Linear;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PairedLayout);

            Init();

            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black);

            GetPairedDevices();
        }

        private void GetPairedDevices()
        {
            List<String> list = new List<string>();
            foreach (BluetoothDevice device in m_PairedDevice)
            {
                list.Add(device.Name + "\n" + device.Address);
            }
            m_Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, list);
            m_ListView.Adapter = m_Adapter;
        }

        public void Init()
        {
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_ListView = FindViewById<ListView>(Resource.Id.listView);
            m_PairedDevice = m_BtAdapter.BondedDevices;
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear2);
        }
    }
}