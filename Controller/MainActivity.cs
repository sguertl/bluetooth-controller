using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Telephony;
using Android.Runtime;
using Android.Net.Sip;

namespace Controller
{
	[Activity(Label = "ControllerActivity",
              Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen",
              MainLauncher = true,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorLandscape
             )]
	public class MainActivity : Activity
	{
        ControllerSettings m_Settings;

		private RadioGroup m_RgControlMethod;
		private RadioButton m_RbThrottleLeft;
		private RadioButton m_RbThrottleRight;
		private TextView m_TvDescription;
		private Button m_BtStart;
        private Button m_BtShowLog;
        private SeekBar m_SbYawTrim;
        private SeekBar m_SbPitchTrim;
        private SeekBar m_SbRollTrim;
        private TextView m_TvYawTrim;
        private TextView m_TvPitchTrim;
        private TextView m_TvRollTrim;

		//private IntentFilter m_Filter; // Used to filter events when searching
		//private CallReciver m_Receiver;

		private bool m_Inverted;
        private int m_YawTrim;

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
            m_BtShowLog = FindViewById<Button>(Resource.Id.btShowLog);
            m_SbYawTrim = FindViewById<SeekBar>(Resource.Id.sbYawTrim);
            m_SbPitchTrim = FindViewById<SeekBar>(Resource.Id.sbPitchTrim);
            m_SbRollTrim = FindViewById<SeekBar>(Resource.Id.sbRollTrim);
            m_TvYawTrim = FindViewById<TextView>(Resource.Id.tvYawTrim);
            m_TvYawTrim.Text = "Yaw Trim ( " + ((m_SbYawTrim.Progress * 2 / 10f) - 10) + " )";
            m_TvPitchTrim = FindViewById<TextView>(Resource.Id.tvPitchTrim);
            m_TvPitchTrim.Text = "Pitch Trim ( " + ((m_SbPitchTrim.Progress * 2 / 10f) - 10) + " )";
            m_TvRollTrim = FindViewById<TextView>(Resource.Id.TvRollTrim);
            m_TvRollTrim.Text = "Roll Trim ( " + ((m_SbRollTrim.Progress * 2 / 10f) - 10) + " )";

            m_Settings = new ControllerSettings();

            m_SbYawTrim.ProgressChanged += (sender, e) => {
                m_TvYawTrim.Text = "Yaw Trim ( " + ((m_SbYawTrim.Progress * 2 / 10f) - 10) + " )";
                m_Settings.TrimYaw = (int)(m_SbYawTrim.Progress * 2 / 10f) - 10;
            };
            m_SbPitchTrim.ProgressChanged += (sender, e) => {
                m_TvPitchTrim.Text = "Pitch Trim ( " + ((m_SbPitchTrim.Progress * 2 / 10f) - 10) + " )";
                m_Settings.TrimPitch = (int)(m_SbPitchTrim.Progress * 2 / 10f) - 10;
            };
            m_SbRollTrim.ProgressChanged += (sender, e) => {
                m_TvRollTrim.Text = "Roll Trim ( " + ((m_SbRollTrim.Progress * 2 / 10f) - 10) + " )";
                m_Settings.TrimRoll = (int)(m_SbRollTrim.Progress * 2 / 10f) - 10;
            };

			m_RbThrottleLeft.Click += OnThrottleLeftClick;
			m_RbThrottleRight.Click += OnThrottleRightClick;

			m_BtStart.SetBackgroundColor(Android.Graphics.Color.DeepSkyBlue);
			m_BtStart.SetTextColor(Android.Graphics.Color.White);
            m_BtShowLog.SetBackgroundColor(Android.Graphics.Color.DeepSkyBlue);
            m_BtShowLog.SetTextColor(Android.Graphics.Color.White);
            			
            m_BtStart.Click += OnStartController;

			//m_Filter = new IntentFilter();

			// m_Receiver = new CallReciver();

			// m_Filter.AddAction("android.intent.action.PHONE_STATE");
			// m_Filter.AddAction("INCOMING_CALL");
			// m_Filter.AddAction(SipSession.State.IncomingCall.ToString());
			// Registering events and forwarding them to the broadcast object
			// RegisterReceiver(m_Receiver, m_Filter);
		}

		private void OnStartController(object sender, EventArgs e)
		{
            m_YawTrim = m_SbYawTrim.Progress;
			var cv = new Controller.ControllerView(this, m_Settings);
			SetContentView(cv);
		}


		private void OnThrottleRightClick(object sender, EventArgs e)
		{
            m_Settings.Inverted = ControllerSettings.ACTIVE;
			m_TvDescription.Text = TEXT_RIGHT;
		}

		private void OnThrottleLeftClick(object sender, EventArgs e)
		{
            m_Settings.Inverted = ControllerSettings.INACTIVE;
			m_TvDescription.Text = TEXT_LEFT;
		}

		private void OnRgClick(object sender, EventArgs e)
		{
			if (m_RbThrottleLeft.Selected) {
				m_Settings.Inverted = ControllerSettings.INACTIVE;
				m_TvDescription.Text = TEXT_LEFT;

			}
			if (m_RbThrottleRight.Selected) {
				m_Settings.Inverted = ControllerSettings.ACTIVE;
				m_TvDescription.Text = TEXT_RIGHT;
			}
		}

		/*private void CheckConnection(object state)
		{
			if (!ConnectedThread.m_Socket.IsConnected) {
				AlertDialog alert = new AlertDialog.Builder(this).Create();
				alert.SetTitle("Connection lost");
				alert.SetMessage("Connection to device lost. Please reconnect.");
				alert.SetButton("Ok", (s, ev) => { });
				alert.Show();
				var intent = new Intent(this, typeof(MainActivity))
					.SetFlags(ActivityFlags.ReorderToFront);
				StartActivity(intent);
			}
		}*/
	}
}
