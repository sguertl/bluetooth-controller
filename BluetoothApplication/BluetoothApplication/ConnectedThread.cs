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
                    //ManageConnectedSocket(m_Socket);
                }
                //
            }
            catch (Java.Lang.Exception connectException)
            {
                try
                {
                    Cancel();
                }
                catch (Java.Lang.Exception closeException) { Console.WriteLine("fail2"); }
                return;
            }

            ManageConnectedSocket(m_Socket);
        }

        private void ManageConnectedSocket(BluetoothSocket mmSocket)
        {
            Sender sender = new Sender(mmSocket);
            sender.Start();
            Java.Lang.String test = new Java.Lang.String("1");
            sender.Write(test.GetBytes());
            Thread.Sleep(100);
            test = new Java.Lang.String("2");
            sender.Write(test.GetBytes());
            Thread.Sleep(100);
            test = new Java.Lang.String("3");
            sender.Write(test.GetBytes());
            Thread.Sleep(100);
            //   Thread.Sleep(2000);
            test = new Java.Lang.String("4");
            sender.Write(test.GetBytes());
            Thread.Sleep(100);
            test = new Java.Lang.String("5");
            sender.Write(test.GetBytes());
            Thread.Sleep(100);
            test = new Java.Lang.String("6");
            sender.Write(test.GetBytes());
            Thread.Sleep(100);

            /*Java.Lang.String test1 = new Java.Lang.String("Dir1:8 Pow1:100 Dir2:3 Pow2:100");
            sender.Write(test1.GetBytes());
            Java.Lang.String test2 = new Java.Lang.String("Dir1:8");
            sender.Write(test2.GetBytes());*/
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