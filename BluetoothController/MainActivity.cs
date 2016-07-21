using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace BluetoothController
{
    [Activity(Label = "BluetoothController", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Light.NoTitleBar")]
    public class MainActivity : Activity
    {
        // Members
        private BluetoothAdapter m_BtAdapter;
        private Button m_BtPairedDevices;
        private Button m_BtSearchDevices;
        private GradientDrawable m_Drawable;
        private LinearLayout m_Linear;
        private bool m_Outside = false;
        private bool m_OutsideSearch = false;
        private Drawable m_Draw;

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
            m_Draw = m_BtPairedDevices.Background;
        //    m_BtPairedDevices.Background = m_Drawable;
       //     m_BtSearchDevices.Background = m_Drawable;
            
            // Setting activity background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);

            // Handling search devices button contact
            m_BtSearchDevices.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                // Setting background blue if button was touched
                if (e2.Event.Action == MotionEventActions.Down)
                {
                    m_BtSearchDevices.SetBackgroundColor(Android.Graphics.Color.Aquamarine);
                }
                // Starting a new activity if button was released
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    if (!m_OutsideSearch)
                    {
                        StartActivity(typeof(SearchDevices));    
                    }
                    m_BtSearchDevices.Background = m_Draw;
                }
                else if(e2.Event.Action == MotionEventActions.Move)
                {
                    if (e2.Event.GetY() + m_BtSearchDevices.GetY() >= m_BtSearchDevices.Top && e2.Event.GetY() + m_BtSearchDevices.GetY() <= m_BtSearchDevices.Bottom &&
                   e2.Event.GetX() >= m_BtSearchDevices.Left && e2.Event.GetX() <= m_BtSearchDevices.Right)
                    {
                        m_OutsideSearch = false;
                        m_BtSearchDevices.SetBackgroundColor(Android.Graphics.Color.Aqua);
                    }
                    else
                    {
                        m_OutsideSearch = true;
                        //  m_BtSearchDevices.Background = m_Drawable; 
                        m_BtSearchDevices.Background = m_Draw;
                    }
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
                    if (!m_Outside)
                    {
                      StartActivity(typeof(PairedDevices));
                    }
                    m_BtPairedDevices.Background = m_Draw;
                }else if(e2.Event.Action == MotionEventActions.Move)
                {
                    if(e2.Event.GetY() + m_BtPairedDevices.GetY()  >=  m_BtPairedDevices.Top && e2.Event.GetY() + m_BtPairedDevices.GetY() <= m_BtPairedDevices.Bottom &&
                       e2.Event.GetX() >= m_BtPairedDevices.Left && e2.Event.GetX() <= m_BtPairedDevices.Right)
                    {
                        m_Outside = false;
                        m_BtPairedDevices.SetBackgroundColor(Android.Graphics.Color.Aqua);
                    }
                    else
                    {
                        m_Outside = true;
                        m_BtPairedDevices.Background = m_Draw;
                    }
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

