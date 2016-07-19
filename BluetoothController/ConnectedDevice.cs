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
        // Member Variablen
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
            // Erzeugen der Objekte
            m_ViewName = FindViewById<TextView>(Resource.Id.DeviceName);
            m_ViewAdresse = FindViewById<TextView>(Resource.Id.DeviceAdresse);
            m_BtControl = FindViewById<Button>(Resource.Id.btSteueren);
            m_BtDisconnect = FindViewById<Button>(Resource.Id.btDisconnect);
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear4);

            // Text Color [Buttons]
            m_BtControl.SetTextColor(Android.Graphics.Color.Black);    // Setzt die Text Color Schwarz
            m_BtDisconnect.SetTextColor(Android.Graphics.Color.Black); // Setzt die Text Color Schwarz

            // Border
            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);               // Formt das Drawable Objekt in ein Rechteckt
            drawable.SetStroke(2, Android.Graphics.Color.Black);  // Setzt die Border Stärke auf 2 und die Farbe Schwarz
            drawable.SetColor(Android.Graphics.Color.White);      // Setzt die Background auf Weiß
            m_BtControl.SetBackgroundDrawable(drawable);          // Button übernimmt das Desgin des Drawable Objekt
            m_BtDisconnect.SetBackgroundDrawable(drawable);       // Button übernimmt das Desgin des Drawable Objekt

            // Activity Background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White); // Setzt die Background Color Weiß

            // Erhält Daten von einer anderen Activity
            IList<String> text = Intent.GetStringArrayListExtra("MyData");
            m_ViewName.Text = text.ElementAt(0);    // Name des verbundenen Geräts
            m_ViewAdresse.Text = text.ElementAt(1); // Adresse des verbundenen Geräts

            // Text Color [Textview]
            m_ViewName.SetTextColor(Android.Graphics.Color.Black);    // Setzt die Text Color Schwarz
            m_ViewAdresse.SetTextColor(Android.Graphics.Color.Black); // Setzt die Text Color Schwarz 

            // On Touch Event
            m_BtControl.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                if (e2.Event.Action == MotionEventActions.Down)
                    m_BtControl.SetBackgroundColor(Android.Graphics.Color.Aquamarine); // Setzt Button Background, wenn der Button berührt wird
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    StartActivity(typeof(ControllerActivity));   // Startet eine neue Activity
                    m_BtControl.SetBackgroundDrawable(drawable); // Setzt das Standart Design vom Button zurück
                }
            };
        }
    }
}