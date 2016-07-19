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

    // ----------------- DOKUMENTATION ------------------

    [Activity(Label = "ConnectedDevice")]
    public class ConnectedDevices : Activity
    {
        private TextView m_ViewName;
        private TextView m_ViewAdresse;
        private Button m_BtControl;
        private Button m_BtDisconnect;
        private LinearLayout m_Linear;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ConnectedDevicesLayout);

            Init();
        }

        public void Init()
        {
            m_ViewName = FindViewById<TextView>(Resource.Id.DeviceName);
            m_ViewAdresse = FindViewById<TextView>(Resource.Id.DeviceAdresse);
            m_BtControl = FindViewById<Button>(Resource.Id.btSteueren);
            m_BtDisconnect = FindViewById<Button>(Resource.Id.btDisconnect);
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear4);

            m_BtControl.SetTextColor(Android.Graphics.Color.Black);
            m_BtDisconnect.SetTextColor(Android.Graphics.Color.Black);

            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke(2, Android.Graphics.Color.Black);

            drawable.SetColor(Android.Graphics.Color.White);
            m_BtControl.SetBackgroundDrawable(drawable);
            m_BtDisconnect.SetBackgroundDrawable(drawable);

            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);

            IList<String> text = Intent.GetStringArrayListExtra("MyData");
            m_ViewName.Text = text.ElementAt(0);
            m_ViewAdresse.Text = text.ElementAt(1);

            m_ViewName.SetTextColor(Android.Graphics.Color.Black);
            m_ViewAdresse.SetTextColor(Android.Graphics.Color.Black);


            m_BtControl.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                if (e2.Event.Action == MotionEventActions.Down)
                    m_BtControl.SetBackgroundColor(Android.Graphics.Color.Aquamarine);
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    StartActivity(typeof(ControllerActivity));
                    m_BtControl.SetBackgroundDrawable(drawable);
                }
            };
        }
    }
}