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

namespace BluetoothController
{
    public class TimeEx : Thread
    {

        public override void Run() 
        {
            Thread.Sleep(10000);

            throw new Java.Lang.Exception();
        }

    }
}