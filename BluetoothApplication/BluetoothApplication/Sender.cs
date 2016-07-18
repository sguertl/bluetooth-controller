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
            Stream tempInStream = null;
            Stream tempOutStream = null;

            try
            {
                tempInStream = socket.InputStream;
                tempOutStream = socket.OutputStream;
            }
            catch(System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("fehler beim erstellen des inputstream");
            }
            m_InputStream = new MemoryStream();
            m_OutputStream = tempOutStream;

            this.Name = "SenderThread";
            m_Handler = handler;
        }

        public override void Run()
        {
            byte[] buffer = new byte[1024];
            int bytes = 0;
            Console.WriteLine("fehler1");
            while (true)
            {
                Console.WriteLine("fehler2");
                try
                {
                    //while (!m_InputStream.IsDataAvailable() || !m_InputStream.CanRead) ;
                    Console.WriteLine("----------- Can read " + m_InputStream.CanRead + " isdataavaiable " + m_InputStream.IsDataAvailable());
                    Console.WriteLine("fehler3");
                    bytes = m_InputStream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine("fehler4");
                    m_Handler.ObtainMessage(1, bytes, -1, buffer);
                }
                catch (Java.Lang.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Fehler beim lesen");
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
                //!!! m_OutputStream.Write(bytes, 0, bytes.Length);
                m_InputStream.Write(bytes, 0, bytes.Length);
            }
            catch(Java.Lang.Exception ex)
            {
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
                m_Socket.Close();
            }
            catch(System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}