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
using Android.Graphics.Drawables;

namespace BluetoothController
{
    public class ConnectedDevices : Activity
    {

        private TextView m_ViewName;
        private TextView m_ViewAdresse;
        private Button m_BtSteuerung;
        private Button m_BtDisconnect;
        private LinearLayout m_Linear;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ConnectedDevicesLayout);

            // Create your application here
            Init();
        }

        public void Init()
        {
            this.m_ViewName = FindViewById<TextView>(Resource.Id.DeviceName);
            this.m_ViewAdresse = FindViewById<TextView>(Resource.Id.DeviceAdresse);
            this.m_BtSteuerung = FindViewById<Button>(Resource.Id.btSteueren);
            this.m_BtDisconnect = FindViewById<Button>(Resource.Id.btDisconnect);
            this.m_Linear = FindViewById<LinearLayout>(Resource.Id.linear4);

            m_BtSteuerung.SetTextColor(Android.Graphics.Color.Black);
            m_BtDisconnect.SetTextColor(Android.Graphics.Color.Black);

            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke(2, Android.Graphics.Color.Black);

            drawable.SetColor(Android.Graphics.Color.White);
            m_BtSteuerung.SetBackgroundDrawable(drawable);
            m_BtDisconnect.SetBackgroundDrawable(drawable);

            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);

            IList<String> text = Intent.GetStringArrayListExtra("MyData");
            m_ViewName.Text = text.ElementAt(0);
            m_ViewAdresse.Text = text.ElementAt(1);

            m_ViewName.SetTextColor(Android.Graphics.Color.Black);
            m_ViewAdresse.SetTextColor(Android.Graphics.Color.Black);
        }



    }
}