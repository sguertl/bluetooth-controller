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
   public class MyBroadcastreciver : BroadcastReceiver
    {
        // Members
        private SearchDevices m_Main;
        private List<String> m_List;
        private List<String> m_CompareList;
        private List<String> m_CopyList;

        public MyBroadcastreciver(SearchDevices main)
        {
            // Initializing objects
            m_Main = main;
            m_List = new List<string>();
            m_CompareList = new List<string>();
        }

        /// <summary>
        /// Clearing the list
        /// </summary>
        public void ResetList()
        {
            m_List = new List<string>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            // Getting specific event
            String action = intent.Action;

            m_Main.GiveAMessage(action);


            if (BluetoothAdapter.ActionDiscoveryStarted.Equals(action))
            {
                m_Main.StartProgress();
            }
            // Checking if search is stopped
          else if (BluetoothAdapter.ActionDiscoveryFinished.Equals(action))
            {
                // Creating a copy of the list
                m_CopyList = new List<string>(m_List);
                if (m_List.Count > 0)
                {
                    m_Main.StartProgress();
                    // Getting address of the device and removing it from the list
                    String address = m_List.ElementAt(0).Split('\n')[1];
                    m_List.RemoveAt(0);

                    // Creating a BluetoothDevice by its address
                    BluetoothDevice device = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
                    bool result = device.FetchUuidsWithSdp();
                }
                else
                {
                    m_Main.Reset();
                }
            }
            // Checking if device was found
            else if ((BluetoothDevice.ActionFound.Equals(action)))
            {
                // Getting the BluetoothDevice from intent
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

                // Adding name and address to device list
                m_List.Add(device.Name + "\n" + device.Address);

                // Add the name and address to an array adapter to show in a Toast
                String derp = device.Name + " - " + device.Address;
                m_Main.GiveAMessage(derp);
            }
            else if (BluetoothDevice.ActionUuid.Equals(action))
            {
                // This is when we can be assured that fetchUuidsWithSdp has completed
                // So get the UUIDs and call fetchUuidsWithSdp on another device in list
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                IParcelable[] uuidExtra = intent.GetParcelableArrayExtra(BluetoothDevice.ExtraUuid);
                try
                {
                    for (int i = 0; i < uuidExtra.Length; i++)
                    {
                        if (i == 0)
                        {
                            if (!m_CompareList.Contains(uuidExtra[i].ToString()))
                            {
                                m_CompareList.Add(uuidExtra[i].ToString());
                                m_Main.AddUuid(uuidExtra[i].ToString());
                            }
                        }
                    }
              

                if (m_List.Count > 0)
                {
                    String address = m_List.ElementAt(0).Split('\n')[1];
                    m_List.RemoveAt(0);
                    BluetoothDevice device2 = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
                    bool result = device2.FetchUuidsWithSdp();
                }
                else { m_Main.SetAdapterToListView(m_CopyList); }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }
    }
}