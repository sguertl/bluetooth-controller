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

namespace BluetoothController
{
    public class DataTransfer
    {
        private Controller.ControllerView m_CV;
        private Sender m_Sender;
        private byte[] m_Bytes;

        /// <summary>
        /// starts reading bytes
        /// </summary>
        public DataTransfer(Controller.ControllerView CV)
        {
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
            m_Bytes = BluetoothController.ByteConverter.ConvertToByte(args);
            m_Sender.Write(m_Bytes);
        }
    }
}