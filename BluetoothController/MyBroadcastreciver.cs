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

    // ---------------- DOKUMENTATION ----------------------

   public class MyBroadcastreciver : BroadcastReceiver
    {
        // Memeber Variable
        private SearchDevices m_Main;
        private List<String> m_List;
        private List<String> m_CompareList;
        private List<String> m_CopyList;

        public MyBroadcastreciver(SearchDevices main)
        {
            // Erzeugen der Objekte
            m_Main = main;
            m_List = new List<string>();
            m_CompareList = new List<string>();
        }

        // Liste wird neu erstellt
        public void ResetList()
        {
            m_List = new List<string>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action; // Gibt an welches Event er gefunden hat

            m_Main.GiveAMessage(action);

            // Wenn das Event ActionDiscoveryFinished eintrifft, ist die Suche beendet
            if (BluetoothAdapter.ActionDiscoveryFinished.Equals(action))
            {
                m_CopyList = new List<string>(m_List); // Erstellt eine Kopie der Liste mit den Devices
                // Fragt ab, ob sich Devices in der Liste befinden
                if (m_List.Count > 0)
                {
                    String address = m_List.ElementAt(0).Split('\n')[1];       // Holt sich die Adresse des Devices
                    m_List.RemoveAt(0);                                        // Löscht dieses Device aus der Liste

                    BluetoothDevice device = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address); // Erstellt ein Bluetooth Objekt mithilfe der Adresse
                    bool result = device.FetchUuidsWithSdp();                                          // Aktiviert das Event ActionUuid
                }
            }
            // Wenn das Event ActionFound eintrifft, ist ein Device gefunden worden
            else if ((BluetoothDevice.ActionFound.Equals(action)))
            {
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice); // Erhält über das intent das BluetoothDevice

                m_List.Add(device.Name + "\n" + device.Address); // Fügt den Namen und die Adresse des Devices der List hinzu

                // Add the name and address to an array adapter to show in a Toast
                String derp = device.Name + " - " + device.Address;
                m_Main.GiveAMessage(derp);
            }
            else if (BluetoothDevice.ActionUuid.Equals(action))
            {
                // This is when we can be assured that fetchUuidsWithSdp has completed.
                // So get the uuids and call fetchUuidsWithSdp on another device in list
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
                else { m_Main.SetAdapterToListView(m_CopyList);}
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }
    }
}