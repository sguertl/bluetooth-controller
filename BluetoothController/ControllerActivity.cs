using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Telephony;
using Android.Runtime;
using Android.Net.Sip;

namespace BluetoothController
{
    [Activity(Label = "ControllerActivity", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorLandscape)]
    public class ControllerActivity : Activity {
        private RadioGroup m_RgControlMethod;
        private RadioButton m_RbThrottleLeft;
        private RadioButton m_RbThrottleRight;
        private TextView m_TvDescription;
        private Button m_BtStart;

        private IntentFilter m_Filter; // Used to filter events when searching
        private CallReciver m_Receiver;

        private bool m_Inverted;

        private readonly String TEXT_LEFT = "The left joystick will be used to regulate throttle and rudder. The right joystick will be used to regulate elevator and aileron.";
        private readonly String TEXT_RIGHT = "The left joystick will be used to regulate elevator and rudder. The right joystick will be used to regulate the throttle and aileron.";

  

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ControllerSettings);

            m_RgControlMethod = FindViewById<RadioGroup>(Resource.Id.rgControlMethod);
            m_RbThrottleLeft = FindViewById<RadioButton>(Resource.Id.rbThrottleLeft);
            m_RbThrottleRight = FindViewById<RadioButton>(Resource.Id.rbThrottleRight);
            m_TvDescription = FindViewById<TextView>(Resource.Id.tvDescription);
            m_BtStart = FindViewById<Button>(Resource.Id.btStart);

            m_RbThrottleLeft.Click += OnThrottleLeftClick;
            m_RbThrottleRight.Click += OnThrottleRightClick;

            m_BtStart.SetBackgroundColor(Android.Graphics.Color.DeepSkyBlue);
            m_BtStart.SetTextColor(Android.Graphics.Color.White);

            m_BtStart.Click += OnStartController;

            m_Filter = new IntentFilter();
         //    m_Receiver = new CallReciver();

         //   m_Filter.AddAction("android.intent.action.PHONE_STATE");
       //     m_Filter.AddAction("INCOMING_CALL");
        //    m_Filter.AddAction(SipSession.State.IncomingCall.ToString());
            // Registering events and forwarding them to the broadcast object
           // RegisterReceiver(m_Receiver, m_Filter);
        }

        private void OnThrottleRightClick(object sender, EventArgs e)
        {
            m_Inverted = true;
            m_TvDescription.Text = TEXT_RIGHT;
        }

        private void OnThrottleLeftClick(object sender, EventArgs e)
        {
            m_Inverted = false;
            m_TvDescription.Text = TEXT_LEFT;
        }

        private void OnStartController(object sender, EventArgs e)
        {
            var cv = new Controller.ControllerView(this, m_Inverted);
            SetContentView(cv);
        }

        private void OnRgClick(object sender, EventArgs e)
        {
            if(m_RbThrottleLeft.Selected)
            {
                m_Inverted = false;
                m_TvDescription.Text = TEXT_LEFT;

            }
            if(m_RbThrottleRight.Selected)
            {
                m_Inverted = true;
                m_TvDescription.Text = TEXT_RIGHT;
            }
        }

        private void CheckConnection(object state)
        {
            if (!ConnectedThread.m_Socket.IsConnected)
            {
                AlertDialog alert = new AlertDialog.Builder(this).Create();
                alert.SetTitle("Connection lost");
                alert.SetMessage("Connection to device lost. Please reconnect.");
                alert.SetButton("Ok", (s, ev) => {});
                alert.Show();
                var intent = new Intent(this, typeof(MainActivity))
                    .SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(intent);
            }
        }
    }
}