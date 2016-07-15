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
    /// BroadcastReceiver for searching other Bluetooth devices
    /// </summary>
    public class MySearchBroadcastReceiver : BroadcastReceiver
    {
        private SearchDevices mMain;
        private List<String> mDeviceList;

        public MySearchBroadcastReceiver(SearchDevices main)
        {
            mMain = main;
            mDeviceList = new List<String>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;

            mMain.GiveAMessage(action);
            //Checks if the device was found and if it finished searching
            if (BluetoothAdapter.ActionDiscoveryFinished.Equals(action))
            {
                mMain.setAdapterToListView(mDeviceList);
            }
            else if ((BluetoothDevice.ActionFound.Equals(action)))
            {
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                // Add the name and address to an array adapter to show in a Toast
                mDeviceList.Add(device.Name + "\n" + device.Address);
                String deviceInfo = device.Name + " - " + device.Address;
                mMain.GiveAMessage(deviceInfo);
            }
        }
    }
}