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

        private BluetoothAdapter m_BtAdapter;
        private BluetoothSocket m_Socket;
        private BluetoothDevice m_Device;
        private MainActivity m_Main;            
        private string m_UuidString;

        public ConnectedThread(BluetoothDevice device, string UUIDString)
        {
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_UuidString = UUIDString;
            MY_UUID = UUID.FromString(m_UuidString);
            // Use a temporary object that is later assigned to m_Socket,
            // because m_Socket is final
            BluetoothSocket tmp = null;
            m_Device = device;

            // Get a BluetoothSocket to connect with the given BluetoothDevice
            /*  try
              {
                  // MY_UUID is the app's UUID string, also used by the server code
                  tmp = device.CreateRfcommSocketToServiceRecord(MY_UUID);
              }
              catch (Java.Lang.Exception e)
                  { }

            */

            tmp = device.CreateRfcommSocketToServiceRecord(MY_UUID);
            Class clazz = tmp.RemoteDevice.Class;
            Class[] paramTypes = new Class[] { Integer.Type };

            Method m = clazz.GetMethod("createRfcommSocket", paramTypes);
            Java.Lang.Object[] param = new Java.Lang.Object[] { Integer.ValueOf(1) };

            m_Socket = (BluetoothSocket)m.Invoke(tmp.RemoteDevice, param);
            // m_Socket.Connect();


            //   m_Socket = tmp;
        }

        public override void Run()
        {
            base.Run();
            // Cancel discovery because it will slow down the connection
            m_BtAdapter.CancelDiscovery();

            try
            {
                // Connect the device through the socket. This will block
                // until it succeeds or throws an exception
                Console.WriteLine("++++++++++++++++++++++++++++++Socket: " + m_Socket.RemoteDevice.Name);
                Console.ReadLine();
                m_Socket.Connect();
            }
            catch (Java.Lang.Exception connectException)
            {
                // Unable to connect; close the socket and get out
                Console.WriteLine(connectException.Message);
                try { m_Socket.Close(); }
                catch (Java.Lang.Exception e) { Console.WriteLine(e.Message); }
                return;
            }

            // Do work to manage the connection (in a separate thread)
            ManageConnectedSocket(m_Socket);
            //  m_Main.getHandler().ObtainMessage(SUCCESS_CONNECT);
        }



        private void ManageConnectedSocket(BluetoothSocket mmSocket)
        {

        }

        // Will cancel an in-progress connection, and close the socket
        public void Cancel()
        {
            try { m_Socket.Close(); }
            catch (Java.Lang.Exception e) { Console.WriteLine(e.Message); }
        }
    }
}