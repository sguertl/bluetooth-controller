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

        public DataTransfer(Controller.ControllerView CV)
        {
            m_CV = CV;
            Init();
        }

        public void Init()
        {
            m_Sender = new Sender(ConnectedThread.m_Socket);
        }

        public void Write()
        {
            // m_Bytes ist noch null
            m_Sender.Write(m_Bytes);
        }


    }
}