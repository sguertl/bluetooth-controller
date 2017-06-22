using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Telephony;
using Android.Runtime;
using Android.Net.Sip;
using Android;
using System.IO;

namespace BluetoothController
{
    [Activity(Label = "ControllerActivity",
              Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen",
              MainLauncher = false,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorLandscape
             )]
    public class ControllerActivity : Activity
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

        private string mStorageDirPath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(BluetoothController.Resource.Layout.ControllerSettings);
            m_RgControlMethod = FindViewById<RadioGroup>(BluetoothController.Resource.Id.rgControlMethod);
            m_RbThrottleLeft = FindViewById<RadioButton>(BluetoothController.Resource.Id.rbThrottleLeft);
            m_RbThrottleRight = FindViewById<RadioButton>(BluetoothController.Resource.Id.rbThrottleRight);
            m_TvDescription = FindViewById<TextView>(BluetoothController.Resource.Id.tvDescription);
            m_BtStart = FindViewById<Button>(BluetoothController.Resource.Id.btStart);
            m_BtShowLog = FindViewById<Button>(BluetoothController.Resource.Id.btShowLog);
            m_SbYawTrim = FindViewById<SeekBar>(BluetoothController.Resource.Id.sbYawTrim);
            m_SbPitchTrim = FindViewById<SeekBar>(BluetoothController.Resource.Id.sbPitchTrim);
            m_SbRollTrim = FindViewById<SeekBar>(BluetoothController.Resource.Id.sbRollTrim);
            m_TvYawTrim = FindViewById<TextView>(BluetoothController.Resource.Id.tvYawTrim);
            m_TvYawTrim.Text = "Yaw Trim ( " + ((m_SbYawTrim.Progress * 2 / 10f) - 10) + " )";
            m_TvPitchTrim = FindViewById<TextView>(BluetoothController.Resource.Id.tvPitchTrim);
            m_TvPitchTrim.Text = "Pitch Trim ( " + ((m_SbPitchTrim.Progress * 2 / 10f) - 10) + " )";
            m_TvRollTrim = FindViewById<TextView>(BluetoothController.Resource.Id.TvRollTrim);
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

            mStorageDirPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "Airything");
            var storageDir = new Java.IO.File(mStorageDirPath);
            storageDir.Mkdirs();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DateTime time = DateTime.Now;
            string logName = string.Format("{0}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}_log", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var writer = new Java.IO.FileWriter(new Java.IO.File(mStorageDirPath, logName + ".csv"));
            writer.Write(DataTransfer.DEBUG);
            writer.Close();
        }

        private void OnStartController(object sender, EventArgs e)
        {
            m_YawTrim = m_SbYawTrim.Progress;
            var cv = new ControllerView(this, m_Settings);
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
            if (m_RbThrottleLeft.Selected)
            {
                m_Settings.Inverted = ControllerSettings.INACTIVE;
                m_TvDescription.Text = TEXT_LEFT;

            }
            if (m_RbThrottleRight.Selected)
            {
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


/****************OLD**************
using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Util;
using System.IO;

namespace BluetoothController
{
    [Activity(Label = "ControllerActivity", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorLandscape)]
    public class ControllerActivity : Activity
    {
        private RadioGroup m_RgControlMethod;
        private RadioButton m_RbThrottleLeft;
        private RadioButton m_RbThrottleRight;
        private TextView m_TvDescription;
        private Button m_BtStart;

        private bool m_Inverted;

        private readonly String TEXT_LEFT = "The left joystick will be used to regulate throttle and rudder. The right joystick will be used to regulate elevator and aileron.";
        private readonly String TEXT_RIGHT = "The left joystick will be used to regulate elevator and rudder. The right joystick will be used to regulate the throttle and aileron.";

        private string storageDirPath;

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

            storageDirPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "Airything");
            var storageDir = new Java.IO.File(storageDirPath);
            storageDir.Mkdirs();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DateTime time = DateTime.Now;
            string logName = string.Format("{0}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}_log", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var writer = new Java.IO.FileWriter(new Java.IO.File(storageDirPath, logName + ".csv"));
            writer.Write(DataTransfer.DEBUG);
            writer.Close();
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
} **********************OLD***************/
