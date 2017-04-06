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

namespace BluetoothController
{
    public class DataTransfer
    {
        private Controller.ControllerView m_CV;
        private Sender m_Sender;
        private byte[] m_Bytes;
        private StreamWriter mWriter;

        /// <summary>
        /// starts reading bytes
        /// </summary>
        public DataTransfer(Controller.ControllerView CV)
        {
            File.SetAttributes(@"C:\Users\AdrianK\Desktop\lala.txt", FileAttributes.Normal);
            mWriter = new StreamWriter(@"C:\Users\AdrianK\Desktop\lala.txt");
            Console.SetOut(mWriter);
            Console.WriteLine("hello");
            m_CV = CV;
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
            data.Remove(data.Length - 1);
            data += "\n";
            mWriter.Write(data);
            Console.SetOut(mWriter);
            m_Bytes = BluetoothController.ByteConverter.ConvertToByte(args);
            m_Sender.Write(m_Bytes);
        }
    }
}