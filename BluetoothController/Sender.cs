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
using System.IO;

namespace BluetoothController
{
    public class Sender : Thread
    {
        // Member Variablen
        private BluetoothSocket m_Socket;
        private Stream m_InputStream;
        private Stream m_OutputStream;
        private string m_Message;
        //

        /// <summary>
        /// Last Message received
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }

        public Sender(BluetoothSocket socket)
        {
            m_Socket = socket;
            Stream tempInStream = null;
            Stream tempOutStream = null;

            try
            {
                tempInStream = socket.InputStream;
                tempOutStream = socket.OutputStream;
            }
            catch (System.Exception ex)
            {
                Cancel();
                Console.WriteLine(ex.Message);
            }

            m_InputStream = tempInStream;
            m_OutputStream = tempOutStream;
        }

        public override void Run()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytes = 0;
                 //   m_InputStream.Position = 0;
                    while (bytes == 0)
                    {
                        bytes += m_InputStream.Read(buffer, 0, buffer.Length);
                    }

                    m_Message = CreateStringFromBuffer(buffer, bytes);
                }
                catch (System.Exception ex)
                {
                    Cancel();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private string CreateStringFromBuffer(byte[] buffer, int bytes)
        {
            string returnString = "";
            for (int i = 0; i < bytes * 8; i++)
            {
                returnString += (char)buffer[i];
            }
            return returnString;
        }

        /// <summary>
        /// Writes byte stream
        /// </summary>
        public void Write(byte[] bytes)
        {
            try
            {
                m_OutputStream.Write(bytes, 0, bytes.Length);
            }
            catch (System.Exception ex)
            {
                Cancel();
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Deactivates reading incoming messages
        /// </summary>
        public void Cancel()
        {
            try
            {
                m_InputStream.Close();
                m_OutputStream.Close();
                m_Socket.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}