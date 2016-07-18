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
using Java.Lang;
using Java.Util;
using Java.Lang.Reflect;

namespace BluetoothApplication
{
    public class ConnectedThread : Thread
    {

        // Member Variablen
        private BluetoothAdapter m_BtAdapter;
        private BluetoothSocket m_Socket;
        private BluetoothDevice m_Device;
        private UUID m_MY_UUID;
        private int m_SUCCESS_CONNECT = 0;
        private string m_UuidString;
        private MyHandler m_Handler;
        //


        public ConnectedThread(BluetoothDevice device, string UUIDString)
        {
            this.Name = "ConnectionThread";
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_UuidString = UUIDString;
            // Der übergebene UUID String wird in ein UUID Objekt konvertiert
            m_MY_UUID = UUID.FromString(m_UuidString);
            //

            BluetoothSocket tmp = null;
            m_Device = device;

            tmp = device.CreateRfcommSocketToServiceRecord(m_MY_UUID);

            // Workaround, damit der Verbindungsaufbau zum Socket funktioniert
            Class testClass = tmp.RemoteDevice.Class;
            Class[] paramTypes = new Class[] { Integer.Type };

            Method m = testClass.GetMethod("createRfcommSocket", paramTypes);
            Java.Lang.Object[] param = new Java.Lang.Object[] { Integer.ValueOf(1) };

            m_Socket = (BluetoothSocket)m.Invoke(tmp.RemoteDevice, param);

            m_Handler = new MyHandler();
            //
        }

        public override void Run()
        {
            // Cancel discovery because it will slow down the connection
            m_BtAdapter.CancelDiscovery();
            //

            try
            {
                // Connect the device through the socket. This will block
                // until it succeeds or throws an exception
                if (!m_Socket.IsConnected)
                {
                    m_Socket.Connect();
                }
                //
            }
            catch (Java.Lang.Exception connectException)
            {
                Console.WriteLine("fail1");
                try
                {
                    Cancel();
                }
                catch (Java.Lang.Exception closeException) { Console.WriteLine("fail2"); }
                return;
            }

            // Do work to manage the connection (in a separate thread)
            ManageConnectedSocket(m_Socket);
            //  main.getHandler().ObtainMessage(SUCCESS_CONNECT);
        }
        //lalala


        private void ManageConnectedSocket(BluetoothSocket mmSocket)
        {
            Sender sender = new Sender(mmSocket, m_Handler);
            sender.Start();
            string test = "lala";
            sender.Write(Convert.FromBase64String(test));
        }

        /// <summary>
        /// Will cancel an in-progress connection, and close the socket
        /// </summary>
        public void Cancel()
        {
            try
            {
                m_Socket.Close();
            }
            catch (Java.Lang.Exception e) { Console.WriteLine("fail3"); }
        }
    }
}