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
        private ListView mListView;
        private BluetoothAdapter mBluetoothAdapter;
        private ICollection<BluetoothDevice> mPairedDevices;
        private ArrayAdapter<String> mDeviceAdapter;
        private LinearLayout linearLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PairedLayout);
            
            init();

            getPairedDevices();
        }

        /// <summary>
        /// Shows paired devices on ListView
        /// </summary>
        private void getPairedDevices()
        {
            List<String> liste = new List<string>();
            foreach (BluetoothDevice device in mPairedDevices)
            {
                liste.Add(device.Name + "\n" + device.Address);
            }
            mDeviceAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, liste);
            mListView.Adapter = mDeviceAdapter;
        }

        /// <summary>
        /// Intializes members and makes the style for ui
        /// </summary>
        public void init()
        {
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            mListView = FindViewById<ListView>(Resource.Id.listView);
            mPairedDevices = mBluetoothAdapter.BondedDevices;
            linearLayout = FindViewById<LinearLayout>(Resource.Id.linear2);

            linearLayout.SetBackgroundColor(Android.Graphics.Color.White);
            mListView.SetBackgroundColor(Android.Graphics.Color.Black);
        }
    }
}