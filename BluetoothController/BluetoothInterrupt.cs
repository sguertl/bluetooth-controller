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
   public class BluetoothInterrupt : Thread
    {
        private bool m_Available = true;

        public override void Run()
        {
            while (true)
            {
                m_Available = true;
                Thread.Sleep(10);
            }
        }

        public void SetAvailable(bool available)
        {
            m_Available = available;
        }

        public bool IsAvailable()
        {
            return m_Available;
        }

    }
}