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

namespace BluetoothApplication
{
    /// <summary>
    /// Messagehandler for processing messages
    /// </summary>
    public class MyHandler : Handler
    {
        private string m_Message;
        private SearchDevices m_SearchDevice;

        public string Message
        {
            get { return m_Message; }
            set { m_Message = value;  }
        }

        public MyHandler(SearchDevices searchDevice)
        {
            m_SearchDevice = searchDevice;
        }

        public override void HandleMessage(Message msg)
        {
            byte[] writeBuffer = (byte[])msg.Obj;
            int begin = msg.Arg1;
            int end = msg.Arg2;

            if(msg.What == 1)
            {
                m_Message = System.Text.Encoding.UTF8.GetString(writeBuffer);
                m_Message = m_Message.Substring(begin, end);
                m_SearchDevice.GiveAMessage(m_Message);
            }
        }
    }
}