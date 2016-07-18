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
using System.IO;
using Java.IO;
using Java.Lang;

namespace BluetoothApplication
{
    public class Sender : Thread
    {
        // Member Variablen
        private BluetoothSocket m_Socket;
        private Stream m_InputStream;
        private Stream m_OutputStream;
        private MyHandler m_Handler;
        //

        public Sender(BluetoothSocket socket, MyHandler handler)
        {
            m_Socket = socket;
            try
            {
                m_InputStream = socket.InputStream;
                m_OutputStream = socket.OutputStream;
            }
            catch(System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            m_Handler = handler;
        }

        public override void Run()
        {
            byte[] buffer = new byte[1024];
            int bytes = 0;
            int begin = 0;

            while (true)
            {
                try
                {
                    bytes += m_InputStream.Read(buffer, bytes, buffer.Length - bytes);
                    for (int i = begin; i < bytes; i++)
                    {
                        if (buffer[i] == Encoding.UTF8.GetBytes("#")[0])
                        {
                            m_Handler.ObtainMessage(1, begin, i, buffer).SendToTarget();
                            begin = i + 1;
                            if (i == bytes - 1)
                            {
                                bytes = 0;
                                begin = 0;
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Writes byte stream
        /// </summary>
        public void Write(byte[] bytes)
        {
            try
            {
                m_OutputStream.Write(bytes, 0, bytes.Length - 1);
            }
            catch(System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Deactivates reading incoming messages
        /// </summary>
        public void Cancel()
        {
            try
            {
                m_Socket.Close();
            }
            catch(System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}