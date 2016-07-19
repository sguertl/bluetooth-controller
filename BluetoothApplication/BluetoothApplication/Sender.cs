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
            catch(System.Exception ex)
            {
                Cancel();             
                Console.WriteLine(ex.Message);
            }

            m_InputStream = new MemoryStream();
            m_OutputStream = tempOutStream;
        }

        public override void Run()
        {
            byte[] buffer = new byte[1024];
            int counter = 0;
            while (counter <200)
            {
                int bytes = 0;
                try
                {
                    while ((bytes = m_InputStream.ReadByte()) != -1)
                    {
                        m_Message += (char)bytes;
                        //bytes += m_InputStream.Read(buffer, 0, buffer.Length);
                    }

                    //m_Message = CreateStringFromBuffer(buffer, bytes);
                    //Console.WriteLine("\nBytes: " + bytes);
                    Console.WriteLine("Message: " + m_Message);
                    Console.WriteLine(counter);
                    m_Message = "";
                    Thread.Sleep(10);
                }
                catch (System.Exception ex)
                {
                    Cancel();
                    Console.Write(ex.Message);
                }
                counter++;
            }
        }

        /*private string CreateStringFromBuffer(byte[] buffer, int bytes)
        {
            string returnString = Encoding.ASCII.GetString(buffer);
            for(int i = 0; i < bytes + 1; i++)
            {
                Console.WriteLine((char)buffer[i] + " ");
            }
            return returnString;
        }*/

        /// <summary>
        /// Writes byte stream
        /// </summary>
        public void Write(byte[] bytes)
        {
            //Console.WriteLine("In write");
            try
            {
                //m_OutputStream.Write(bytes, 0, bytes.Length);  
                //Console.WriteLine("wird gesendet");   
                //m_InputStream.Write(bytes, 0, bytes.Length);  
                for(int i = 0; i < bytes.Length; i++)
                {
                    m_InputStream.WriteByte(bytes[i]);
                }  
            }
            catch(System.Exception ex)
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
                m_Socket.Close();
            }
            catch(System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Activity activity = new Activity();
                activity.StartActivity(typeof(SearchDevices));
            }
        }
    }
}