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
using Java.Lang;
using Android.Bluetooth;
using Java.Util;
using Java.Lang.Reflect;

namespace BluetoothController
{

    // --------------- DOKUMENTATION, FORMATIERUNG --------------------

    public class ConnectedThread : Thread
    {
        // Constants
        private readonly int SUCCESS_CONNECT = 0;
        private readonly UUID MY_UUID;

        // Member Variablen
        private BluetoothAdapter m_BtAdapter;
        private BluetoothSocket m_Socket;
        private BluetoothDevice m_Device;
        private MainActivity m_Main;            
        private string m_UuidString;
        private Sender m_Sender;

        public ConnectedThread(BluetoothDevice device, string UUIDString)
        {
            // Erzeugen der Objekte
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_UuidString = UUIDString;

            
            MY_UUID = UUID.FromString(m_UuidString); // Wandelt den UUID String in ein UUID Objekt um

            // Use a temporary object that is later assigned to m_Socket
            BluetoothSocket tmp = null;
            m_Device = device;

            // Get a BluetoothSocket to connect with the given BluetoothDevice      
            // Workaround to get the Bluetoothsocket
            tmp = device.CreateRfcommSocketToServiceRecord(MY_UUID);
            Class testClass = tmp.RemoteDevice.Class;
            Class[] paramTypes = new Class[] { Integer.Type };

            Method m = testClass.GetMethod("createRfcommSocket", paramTypes);
            Java.Lang.Object[] param = new Java.Lang.Object[] { Integer.ValueOf(1) };
            //

            m_Socket = (BluetoothSocket)m.Invoke(tmp.RemoteDevice, param);
        }

        public override void Run()
        {
            m_BtAdapter.CancelDiscovery(); // Beendet die Suche nach Devices, sonst würde sich der Verbindungaufbau verlangsamen
            try
            {

                // Prüft ob bereits das Device verbunden ist
                if (!m_Socket.IsConnected) { m_Socket.Connect(); }      
            }
            catch (Java.Lang.Exception connectException)
            {
                // Kann sich nicht mit dem Device Verbinden
                Console.WriteLine(connectException.Message);
                try { m_Socket.Close(); }
                catch (Java.Lang.Exception e) { Console.WriteLine(e.Message); }
                return;
            }

           
            ManageConnectedSocket(m_Socket);
        }

        ´/// <summary>
        /// Start der Datenübertragung
        /// </summary>
        /// <param name="mmSocket"></param>
        private void ManageConnectedSocket(BluetoothSocket mmSocket)
        {
            m_Sender = new Sender(mmSocket);
            m_Sender.Start();
            //Java.Lang.String test = new Java.Lang.String("Dir1:8 Pow1:100 Dir2:3 Pow2:100");
            //sender.Write(test.GetBytes());
        }

        public void Write(int PowerLeft, int DirectionLeft, int PowerRight, int DirectionRight)
        {
            Java.Lang.String power = new Java.Lang.String(PowerLeft+":"+DirectionLeft+":"+PowerRight+":"+DirectionRight);
            m_Sender.Write(power.GetBytes());
        }

        // Will cancel an in-progress connection, and close the socket
        public void Cancel()
        {
            try { m_Socket.Close(); }
            catch (Java.Lang.Exception e) { Console.WriteLine(e.Message); }
        }
    }
}