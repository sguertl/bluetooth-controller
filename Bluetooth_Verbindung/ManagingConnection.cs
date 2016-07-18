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
using Java.IO;
using System.IO;
using Java.Lang;

namespace Bluetooth_Verbindung
{
   public class ManagingConnection :Thread
    {
        private  BluetoothSocket mmSocket;
        private  Stream mmInStream;
        private  Stream mmOutStream;
 
    public ManagingConnection(BluetoothSocket socket)
        {
            mmSocket = socket;
            Stream tmpIn = null;
            Stream tmpOut = null;

            // Get the input and output streams, using temp objects because
            // member streams are final
            try
            {
                tmpIn = socket.InputStream;
                tmpOut = socket.OutputStream; 
            }
            catch (Java.Lang.Exception e) { }

            mmInStream = tmpIn;
            mmOutStream = tmpOut;
        }

        public override void Run()
        {
            byte[] buffer = new byte[1024];  // buffer store for the stream
            int bytes = 0; // bytes returned from read()

            // Keep listening to the InputStream until an exception occurs
            while (true)
            {
                try
                {
                    while (!mmInStream.CanRead || !mmInStream.IsDataAvailable())
                    {
                        System.Console.WriteLine("Can Read: " + mmInStream.CanRead + " Available: " + mmInStream.IsDataAvailable() );
                        Thread.Sleep(5000);
                    }
                    
                    bytes = mmInStream.Read(buffer, 0, buffer.Length);

                    //mHandler.obtainMessage(1, bytes, -1, buffer).sendToTarget();
                    System.Console.WriteLine("");
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        /* Call this from the main activity to send data to the remote device */
        public void write(byte[] bytes)
        {
            try
            {
                mmOutStream.Write(bytes, 0, bytes.Length - 1);
            }
            catch (Java.Lang.Exception e) { }
        }

        /* Call this from the main activity to shutdown the connection */
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