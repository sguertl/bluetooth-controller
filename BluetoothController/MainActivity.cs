using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Graphics.Drawables;

namespace BluetoothController
{

    // ------------- DOKUMENTATION, FORMATIERUNG -------------------

    [Activity(Label = "BluetoothController", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // Member Variablen
        private BluetoothAdapter m_BtAdapter;
        private Button m_BtPairedDevices;
        private Button m_BtSearchDevices;
        private GradientDrawable m_Drawable;
        private LinearLayout m_Linear;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            //ಠ_ಠ
            Init();

            // Fragt ab ob der BluetoothAdapter null ist
            // Wenn dies zutrifft wird Bluetooth am jeweiligen Device nicht unterstützt
            if (m_BtAdapter == null)
            {
                Toast.MakeText(ApplicationContext, "Bluetooth is not supported", 0).Show();
                m_BtSearchDevices.Enabled = false;
                m_BtSearchDevices.SetTextColor(Android.Graphics.Color.LightGray);
                m_BtPairedDevices.Enabled = false;
                m_BtPairedDevices.SetTextColor(Android.Graphics.Color.LightGray);

                AlertDialog alert = new AlertDialog.Builder(this).Create();
                alert.SetTitle("Bluetooth not supported");
                alert.SetMessage("Bluetooth is not supported!");
                alert.SetButton("Ok", (s, ev) =>
                {
                    Finish();
                });
                alert.Show();

            }
            else
            {
                // Fragt ab ob Bluetooth eingeschaltet ist
                if (!m_BtAdapter.IsEnabled)
                {
                    // Bluetoothaktievierungs Anfrage
                    TurnBTOn();
                }
            }
        }

        public void Init()
        {
            // Erzeugen der Objekte
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_BtPairedDevices = FindViewById<Button>(Resource.Id.btPairedDevices);
            m_BtSearchDevices = FindViewById<Button>(Resource.Id.btSearchDevices);
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear);

            // Text Color [Button]
            m_BtPairedDevices.SetTextColor(Android.Graphics.Color.Black); // Setzt die Text Color Schwarz
            m_BtSearchDevices.SetTextColor(Android.Graphics.Color.Black); // Setzt die Text Color Schwarz

            // Border
            m_Drawable = new GradientDrawable();
            m_Drawable.SetShape(ShapeType.Rectangle);               // Formt das Drawable Objekt in ein Rechteckt
            m_Drawable.SetStroke(2, Android.Graphics.Color.Black);  // Setzt die Border Stärke auf 2 und die Farbe Schwarz
            m_Drawable.SetColor(Android.Graphics.Color.White);      // Setzt die Background auf Weiß
            m_BtPairedDevices.SetBackgroundDrawable(m_Drawable);    // Button übernimmt das Desgin des Drawable Objekt
            m_BtSearchDevices.SetBackgroundDrawable(m_Drawable);    // Button übernimmt das Desgin des Drawable Objekt

            // Activity Background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White); // Setzt die Background Color Weiß

            // On Touch Event
            m_BtSearchDevices.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
        {
            if (e2.Event.Action == MotionEventActions.Down)
                m_BtSearchDevices.SetBackgroundColor(Android.Graphics.Color.Aquamarine); // Setzt Button Background, wenn der Button berührt wird
            else if (e2.Event.Action == MotionEventActions.Up)
            {
                StartActivity(typeof(SearchDevices));                // Startet eine neue Activity
                m_BtSearchDevices.SetBackgroundDrawable(m_Drawable); // Setzt das Standart Design vom Button zurück
            }
        };

            // On Touch Event
            m_BtPairedDevices.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                if (e2.Event.Action == MotionEventActions.Down)
                    m_BtPairedDevices.SetBackgroundColor(Android.Graphics.Color.Aquamarine);  // Setzt Button Background, wenn der Button berührt wird
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    StartActivity(typeof(PairedDevices));                // Startet eine neue Activity
                    m_BtPairedDevices.SetBackgroundDrawable(m_Drawable); // Setzt das Standart Design vom Button zurück
                }
            };

        }

        // Schaltet Bluetooth ein
        public void TurnBTOn()
        {
            Intent intent = new Intent(BluetoothAdapter.ActionRequestEnable);
            StartActivityForResult(intent, 1);
        }
    }
}

