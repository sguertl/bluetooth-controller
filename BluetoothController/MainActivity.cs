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
        private BluetoothAdapter m_BtAdapter;
        private Button m_BtPairedDevices;
        private Button m_BtSearchDevices;
        private GradientDrawable drawable;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Init();

            if (m_BtAdapter == null)
            {
                Toast.MakeText(ApplicationContext, "Bluetooth is not supported", 0).Show();
            }
            else
            {
                if (!m_BtAdapter.IsEnabled)
                {
                    TurnBTOn();
                }
            }
        }

        public void Init()
        {
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_BtPairedDevices = FindViewById<Button>(Resource.Id.btPairedDevices);
            m_BtSearchDevices = FindViewById<Button>(Resource.Id.btSearchDevices);

            // Text Color
            m_BtPairedDevices.SetTextColor(Android.Graphics.Color.Black);
            m_BtSearchDevices.SetTextColor(Android.Graphics.Color.Black);

            // Border
            drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke(2, Android.Graphics.Color.Black);

            drawable.SetColor(Android.Graphics.Color.White);
            m_BtPairedDevices.SetBackgroundDrawable(drawable);
            m_BtSearchDevices.SetBackgroundDrawable(drawable);

            // Main Background
            LinearLayout line = FindViewById<LinearLayout>(Resource.Id.linear);
            line.SetBackgroundColor(Android.Graphics.Color.White);

        m_BtSearchDevices.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
        {
            if (e2.Event.Action == MotionEventActions.Down)
                m_BtSearchDevices.SetBackgroundColor(Android.Graphics.Color.Aquamarine);
            else if (e2.Event.Action == MotionEventActions.Up)
            {
                StartActivity(typeof(SearchDevices));
                m_BtSearchDevices.SetBackgroundDrawable(drawable);
            }
        };

            m_BtPairedDevices.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                if (e2.Event.Action == MotionEventActions.Down)
                    m_BtPairedDevices.SetBackgroundColor(Android.Graphics.Color.Aquamarine);
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    StartActivity(typeof(PairedDevices));
                    m_BtPairedDevices.SetBackgroundDrawable(drawable);
                }
            };

        }

        public void TurnBTOn()
        {
            Intent intent = new Intent(BluetoothAdapter.ActionRequestEnable);
            StartActivityForResult(intent, 1);
        }
    }
}

