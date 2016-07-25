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
        // Members
        private BluetoothSocket m_Socket;
        private Stream m_InputStream;
        private Stream m_OutputStream;
        private static Int16[] m_Message;

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

        /// <summary>
        /// Start to read bytes
        /// </summary>
        public override void Run()
        {
            // byte buffer 11 - 1 byte as start byte - data bytes - last to bytes fcs
            byte[] buffer = new byte[11];
            while (true)
            {
                try
                {
                    int bytes = 0;
                    // m_InputStream.Position = 0;
                    //only processes bytes when byte were sent
                    while (bytes == 0)
                    {
                        bytes += m_InputStream.Read(buffer, 0, buffer.Length);
                    }
                    //checks if the message was correctly sent
                    m_Message = CheckBuffer(buffer);
                    //Test
                    /*Console.Write("Receive: ");
                    for(int i = 0; i < m_Message.Length; i++)
                    {
                        Console.Write(m_Message[i] + " ");
                    }
                    Console.WriteLine("");*/
                }
                catch (System.Exception ex)
                {
                    Cancel();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private Int16[] CheckBuffer(byte[] buffer)
        {
            if(buffer[0] == ByteConverter.STARTBYTE)
            {
                return ByteConverter.ConvertFromByte(buffer);
            }
            else
            {
                for(int i = 1; i < buffer.Length; i++)
                {
                    if(buffer[i] == ByteConverter.STARTBYTE)
                    {
                        return ByteConverter.ConvertFromByte(GetArrayFromPosition(buffer, i));
                    }
                }
            }
            return null;
        }

        private byte[] GetArrayFromPosition(byte[] bytes, int position)
        {
            byte[] newBytes = new byte[bytes.Length - position];
            for(int i = position; i < newBytes.Length; i++)
            {
                newBytes[i - position] = bytes[i];
            }
            return newBytes;
        }

        /// <summary>
        /// Writes byte stream
        /// </summary>
        public void Write(byte[] bytes)
        {
            try
            {
                /*for(int i = 0; i < bytes.Length; i++)
                {
                    m_OutputStream.WriteByte(bytes[i]);
                }*/
                m_OutputStream.Write(bytes, 0, bytes.Length);
                //TEST
                /*for (int i = 0; i < bytes.Length; i++)
                {
                    Console.WriteLine(bytes[i] + " ");
                }
                Console.WriteLine("");*/
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