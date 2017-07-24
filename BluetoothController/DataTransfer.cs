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
using Android.Util;
using Java.IO;

namespace BluetoothController
{
    public class DataTransfer
    {
        private Sender m_Sender;
        private byte[] m_Bytes;

        //DEBUG
        public static string DEBUG;

        /// <summary>
        /// starts reading bytes
        /// </summary>
        public DataTransfer()
        {
            Init();
        }

        private void Init()
        {
            m_Sender = new Sender(ConnectedThread.m_Socket);
            m_Sender.Start();
        }

        /// <summary>
        /// sends data from joystick
        /// </summary>
        /// <param name="args">(throttle, rotation, forward/backward, left/right)</param>
        public void Write(params Int16[] args)
        {
            string data = "";
            for(int i = 0; i < args.Length; i++)
            {
                data += args[i] + ";";
            }
            data = data.Remove(data.Length - 1);
            
            DEBUG += (data + '\n');
            m_Bytes = ByteConverter.ConvertToByte(args);
            m_Sender.Write(m_Bytes);
        }
    }
}