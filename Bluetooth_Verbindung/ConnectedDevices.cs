using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

using Android.Graphics.Drawables;
using Android.Widget;

namespace Bluetooth_Verbindung
{
    [Activity(Label = "ConnectedDevices", ScreenOrientation = Android.Content.PM.ScreenOrientation.UserLandscape)]
    public class ConnectedDevices : Activity
    {

        private TextView viewName;
        private TextView viewAdresse;
        private Button btSteuerung;
        private Button btDisconnect;
        private LinearLayout linear;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ConnectedDevicesLayout);

            // Create your application here
            Init();
        }

        public void Init()
        {
            this.viewName = FindViewById<TextView>(Resource.Id.DeviceName);
            this.viewAdresse = FindViewById<TextView>(Resource.Id.DeviceAdresse);
            this.btSteuerung = FindViewById<Button>(Resource.Id.btSteueren);
            this.btDisconnect = FindViewById<Button>(Resource.Id.btDisconnect);
            this.linear = FindViewById<LinearLayout>(Resource.Id.linear4);

            btSteuerung.SetTextColor(Android.Graphics.Color.Black);
            btDisconnect.SetTextColor(Android.Graphics.Color.Black);

            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke(2, Android.Graphics.Color.Black);

            drawable.SetColor(Android.Graphics.Color.White);
            btSteuerung.SetBackgroundDrawable(drawable);
            btDisconnect.SetBackgroundDrawable(drawable);

            linear.SetBackgroundColor(Android.Graphics.Color.White);

            IList<String> text = Intent.GetStringArrayListExtra("MyData");
            viewName.Text = text.ElementAt(0);
            viewAdresse.Text = text.ElementAt(1);

            viewName.SetTextColor(Android.Graphics.Color.Black);
            viewAdresse.SetTextColor(Android.Graphics.Color.Black);
        }

    }
}