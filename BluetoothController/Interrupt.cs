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
   public class Interrupt : Thread
    {
        private bool m_Verfuegbar = true;

        public override void Run()
        {
            while (true)
            {
                m_Verfuegbar = true;
                Thread.Sleep(10);
            }
        }

        public void SetVerfuegbar(bool t)
        {
            m_Verfuegbar = t;
        }

        public bool GetVerfuegbar()
        {
            return m_Verfuegbar;
        }

    }
}