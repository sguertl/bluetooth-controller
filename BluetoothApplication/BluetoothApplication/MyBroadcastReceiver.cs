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
    public class MyBroadcastReceiver : BroadcastReceiver
    {
        private SearchDevices main;
        private List<String> liste;

        public MyBroadcastReceiver(SearchDevices main)
        {
            this.main = main;
            this.liste = new List<string>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            // Gibt einmal Started und dann Finished
            String action = intent.Action;

            main.GiveAMessage(action);

            if (BluetoothAdapter.ActionDiscoveryFinished.Equals(action))
            {
                main.setAdapterToListView(liste);
            }
            else if ((BluetoothDevice.ActionFound.Equals(action)))
            {
                //  BluetoothDevice device = intent.ParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                // Add the name and address to an array adapter to show in a Toast
                liste.Add(device.Name + "\n" + device.Address);
                String derp = device.Name + " - " + device.Address;
                main.GiveAMessage(derp);
            }
        }
    }
}