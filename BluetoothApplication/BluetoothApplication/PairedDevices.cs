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
    [Activity(Label = "PairedDevices")]
    public class PairedDevices : Activity
    {
        private ListView listView;
        private BluetoothAdapter btAdapter;
        private ICollection<BluetoothDevice> pairedDevice;
        private ArrayAdapter<String> adapter;
        private LinearLayout linear;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PairedLayout);

            init();

            linear.SetBackgroundColor(Android.Graphics.Color.White);
            listView.SetBackgroundColor(Android.Graphics.Color.Black);

            getPairedDevices();
        }

        private void getPairedDevices()
        {
            List<String> liste = new List<string>();
            foreach (BluetoothDevice device in pairedDevice)
            {
                liste.Add(device.Name + "\n" + device.Address);
            }
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, liste);
            listView.Adapter = adapter;
        }

        public void init()
        {
            btAdapter = BluetoothAdapter.DefaultAdapter;
            listView = FindViewById<ListView>(Resource.Id.listView);
            pairedDevice = btAdapter.BondedDevices;
            linear = FindViewById<LinearLayout>(Resource.Id.linear2);
        }
    }
}