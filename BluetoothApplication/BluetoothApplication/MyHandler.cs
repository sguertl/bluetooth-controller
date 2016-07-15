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
        public override void HandleMessage(Message msg)
        {
            byte[] writeBuffer = (byte[])msg.Obj;
            int begin = msg.Arg1;
            int end = msg.Arg2;

            if(msg.What == 1)
            {
                string message = System.Text.Encoding.UTF8.GetString(writeBuffer);
                message = message.Substring(begin, end);
            }
        }
    }
}