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
using System.Threading;

namespace BluetoothApplication
{
    public class Sender
    {
        private BluetoothSocket mSocket;
        private Stream mInputStream;
        private Stream mOutputStream;
        private Thread mThread;
        private MyHandler mHandler;

        public Sender(BluetoothSocket socket)
        {
            mSocket = socket;
            try
            {
                mInputStream = socket.InputStream;
                mOutputStream = socket.OutputStream;
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            mHandler = new MyHandler();

            mThread = new Thread(() =>
            {
                byte[] buffer = new byte[1024];
                int bytes = 0;
                int begin = 0;

                while (true)
                {
                    try
                    {
                        bytes += mInputStream.Read(buffer, bytes, buffer.Length - bytes);
                        for(int i = begin; i < bytes; i++)
                        {
                            if(buffer[i] == System.Text.Encoding.UTF8.GetBytes("#")[0])
                            {
                                mHandler.ObtainMessage(1, begin, i, buffer).SendToTarget();
                                begin = i + 1;
                                if(i == bytes - 1)
                                {
                                    bytes = 0;
                                    begin = 0;
                                }
                            }
                        }
                    }catch(Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }
            });
        }

        /// <summary>
        /// Starts to read incoming data. (Has to be closed with cancel())
        /// </summary>
        public void Read()
        {
            mThread.Start();
        }

        /// <summary>
        /// Writes byte stream
        /// </summary>
        public void Write(byte[] bytes)
        {
            try
            {
                mOutputStream.Write(bytes, 0, bytes.Length - 1);
            }
            catch(Exception ex)
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
                mSocket.Close();
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}