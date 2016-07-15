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
        private BluetoothAdapter btAdapter;
        private BluetoothSocket mmSocket;
        private BluetoothDevice mmDevice;
        private MainActivity main;
        private UUID MY_UUID;
        private int SUCCESS_CONNECT = 0;
        private string uuidString;


        public ConnectedThread(BluetoothDevice device, string UUIDString)
        {
            this.btAdapter = BluetoothAdapter.DefaultAdapter;
            this.uuidString = UUIDString;
            MY_UUID = UUID.FromString(uuidString);
            // Use a temporary object that is later assigned to mmSocket,
            // because mmSocket is final
            BluetoothSocket tmp = null;
            mmDevice = device;

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

            mmSocket = (BluetoothSocket)m.Invoke(tmp.RemoteDevice, param);
            // mmSocket.Connect();


            //   mmSocket = tmp;
        }

        public override void Run()
        {
            base.Run();
            // Cancel discovery because it will slow down the connection
            btAdapter.CancelDiscovery();

            try
            {
                // Connect the device through the socket. This will block
                // until it succeeds or throws an exception
                Console.WriteLine("++++++++++++++++++++++++++++++Socket: " + mmSocket.RemoteDevice.Name);



                Console.ReadLine();

                mmSocket.Connect();
            }
            catch (Java.Lang.Exception connectException)
            {
                // Unable to connect; close the socket and get out
                Console.WriteLine(connectException.Message);
                try
                {
                    mmSocket.Close();
                }
                catch (Java.Lang.Exception closeException) { }
                return;
            }

            // Do work to manage the connection (in a separate thread)
            manageConnectedSocket(mmSocket);
            //  main.getHandler().ObtainMessage(SUCCESS_CONNECT);
        }



        private void manageConnectedSocket(BluetoothSocket mmSocket)
        {

        }

        /** Will cancel an in-progress connection, and close the socket */
        public void cancel()
        {
            try
            {
                mmSocket.Close();
            }
            catch (Java.Lang.Exception e) { }
        }
    }
}