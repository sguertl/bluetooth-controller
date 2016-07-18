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

namespace BluetoothApplication
{
    /// <summary>
    /// Shows all paired devices
    /// </summary>
    [Activity(Label = "PairedDevices")]
    public class PairedDevices : Activity
    {
        // Member Variablen
        private ListView m_ListView;
        private BluetoothAdapter m_BluetoothAdapter;
        private ICollection<BluetoothDevice> m_PairedDevices;
        private ArrayAdapter<String> m_DeviceAdapter;
        private LinearLayout m_LinearLayout;
        //

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PairedLayout);
            Init();
            GetPairedDevices();
        }

        /// <summary>
        /// Shows paired devices on ListView
        /// </summary>
        private void GetPairedDevices()
        {
            List<String> liste = new List<string>();
            foreach (BluetoothDevice device in m_PairedDevices)
            {
                liste.Add(device.Name + "\n" + device.Address);
            }
            m_DeviceAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, liste);
            m_ListView.Adapter = m_DeviceAdapter;
        }

        /// <summary>
        /// Intializes members and makes the style for ui
        /// </summary>
        public void Init()
        {
            m_BluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            m_ListView = FindViewById<ListView>(Resource.Id.listView);

            m_PairedDevices = m_BluetoothAdapter.BondedDevices;

            m_LinearLayout = FindViewById<LinearLayout>(Resource.Id.linear2);

            m_LinearLayout.SetBackgroundColor(Android.Graphics.Color.White);
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black);
        }
    }
}