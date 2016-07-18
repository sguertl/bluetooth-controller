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
        // Member Variablen
        private SearchDevices m_Main;
        private List<String> m_Liste;
        private List<String> m_CompareList;
        //

        public MySearchBroadcastReceiver(SearchDevices main)
        {
            this.m_Main = main;
            this.m_Liste = new List<string>();
            this.m_CompareList = new List<string>();
        }

        public void setListNeu()
        {
            m_Liste = new List<string>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            // Gibt einmal Started und dann Finished
            String action = intent.Action;

            m_Main.GiveAMessage(action);

            if (BluetoothAdapter.ActionDiscoveryFinished.Equals(action))
            {
                m_Main.setAdapterToListView(m_Liste);
                if (m_Liste.Count > 0)
                {
                    String address = m_Liste.ElementAt(0).Split('\n')[1];
                    m_Liste.RemoveAt(0);
                    BluetoothDevice device = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
                    bool result = device.FetchUuidsWithSdp();
                }
            }
            else if ((BluetoothDevice.ActionFound.Equals(action)))
            {
                //  BluetoothDevice device = intent.ParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                // Add the name and address to an array adapter to show in a Toast
                m_Liste.Add(device.Name + "\n" + device.Address);
                String derp = device.Name + " - " + device.Address;
                m_Main.GiveAMessage(derp);
            }
            else if (BluetoothDevice.ActionUuid.Equals(action))
            {
                // This is when we can be assured that fetchUuidsWithSdp has completed.
                // So get the uuids and call fetchUuidsWithSdp on another device in list
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                IParcelable[] uuidExtra = intent.GetParcelableArrayExtra(BluetoothDevice.ExtraUuid);
                for (int i = 0; i < uuidExtra.Length; i++)
                {
                    if (i == 0)
                    {
                        if (!m_CompareList.Contains(uuidExtra[i].ToString()))
                        {
                            m_CompareList.Add(uuidExtra[i].ToString());
                            m_Main.AddUUid(uuidExtra[i].ToString());
                        }

                    }
                }
                if (m_Liste.Count > 0)
                {
                    String address = m_Liste.ElementAt(0).Split('\n')[1];
                    m_Liste.RemoveAt(0);
                    BluetoothDevice device2 = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
                    bool result = device2.FetchUuidsWithSdp();
                }
            }
        }
    }
}