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

namespace Bluetooth_Verbindung
{
    public class MyBroadcastreciver : BroadcastReceiver
    {
        private SearchDevices main;
        private List<String> liste;

        public MyBroadcastreciver(SearchDevices main)
        {
            this.main = main;
            this.liste = new List<string>();
        }

        public void setListNeu()
        {
            liste = new List<string>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            // Gibt einmal Started und dann Finished
            String action = intent.Action;

            main.GiveAMessage(action);

            if (BluetoothAdapter.ActionDiscoveryFinished.Equals(action))
            {
                main.setAdapterToListView(liste);
                if (liste.Count > 0)
                {
                    String address = liste.ElementAt(0).Split('\n')[1];
                    liste.RemoveAt(0);
                    BluetoothDevice device = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
                    bool result = device.FetchUuidsWithSdp();
                }
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
            else if (BluetoothDevice.ActionUuid.Equals(action))
            {
                // This is when we can be assured that fetchUuidsWithSdp has completed.
                // So get the uuids and call fetchUuidsWithSdp on another device in list
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                IParcelable[] uuidExtra = intent.GetParcelableArrayExtra(BluetoothDevice.ExtraUuid);
                Console.WriteLine("///////////////////////////////Device: " + device.Name + " " + device.Address);
                for (int i = 0; i < uuidExtra.Length; i++)
                {
                    if (i == 0)
                    {
                        Console.WriteLine("///////////////////////////////" + uuidExtra[i].ToString());
                        main.AddUUid(uuidExtra[i].ToString());
                    }
                }

            }
        }
    }
}