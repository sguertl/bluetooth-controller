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
    [Activity(Label = "BluetoothController", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // Members
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

            // Checking if bluetooth is supported
            if (m_BtAdapter == null)
            {
                Toast.MakeText(ApplicationContext, "Bluetooth is not supported", 0).Show();
                m_BtSearchDevices.Enabled = false;
                m_BtSearchDevices.SetTextColor(Android.Graphics.Color.LightGray);
                m_BtPairedDevices.Enabled = false;
                m_BtPairedDevices.SetTextColor(Android.Graphics.Color.LightGray);

                // Displaying an alert to inform the user that bluetooth is not supported
                AlertDialog alert = new AlertDialog.Builder(this).Create();
                alert.SetTitle("Bluetooth not supported");
                alert.SetMessage("Bluetooth is not supported!");
                alert.SetButton("Ok", (s, ev) => { Finish(); });
                alert.Show();

            }
            else
            {
                // Checking if bluetooth is enabled
                if (!m_BtAdapter.IsEnabled) { TurnBTOn(); }
            }
        }

        /// <summary>
        /// Initializing and modifies objects
        /// </summary>
        public void Init()
        {
            // Initializing objects
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_BtPairedDevices = FindViewById<Button>(Resource.Id.btPairedDevices);
            m_BtSearchDevices = FindViewById<Button>(Resource.Id.btSearchDevices);
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear);

            // Setting text color of the buttons
            m_BtPairedDevices.SetTextColor(Android.Graphics.Color.Black);
            m_BtSearchDevices.SetTextColor(Android.Graphics.Color.Black);

            // Setting border
            m_Drawable = new GradientDrawable();
            m_Drawable.SetShape(ShapeType.Rectangle);
            m_Drawable.SetStroke(2, Android.Graphics.Color.Black);
            m_Drawable.SetColor(Android.Graphics.Color.White);
            m_BtPairedDevices.Background = m_Drawable;
            m_BtSearchDevices.Background = m_Drawable;

            // Setting activity background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);

            // Handling search devices button contact
            m_BtSearchDevices.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                // Setting background blue if button was touched
                if (e2.Event.Action == MotionEventActions.Down)
                {
                    m_BtSearchDevices.SetBackgroundColor(Android.Graphics.Color.Aquamarine); // Setzt Button Background, wenn der Button berührt wird
                }
                // Starting a new activity if button was released
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    StartActivity(typeof(SearchDevices));
                    m_BtSearchDevices.Background = m_Drawable;
                }
            };

            // Handling paired devices button contact
            m_BtPairedDevices.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                // Setting background blue if button was touched
                if (e2.Event.Action == MotionEventActions.Down)
                { 
                    m_BtPairedDevices.SetBackgroundColor(Android.Graphics.Color.Aquamarine);
                }
                // Starting a new activity if button was released
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    StartActivity(typeof(PairedDevices));
                    m_BtPairedDevices.Background = m_Drawable;
                }
            };
        }

        /// <summary>
        /// Enables bluetooth on the device
        /// </summary>
        public void TurnBTOn()
        {
            Intent intent = new Intent(BluetoothAdapter.ActionRequestEnable);
            StartActivityForResult(intent, 1);
        }
    }
}

