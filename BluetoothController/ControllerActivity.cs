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

        private bool m_Inverted = false;

        private readonly String TEXT_LEFT = "The left joystick will be used to regulate the throttle and rotation. The right joystick will be used to move forward, backwards, to the right and to the left.";
        private readonly String TEXT_RIGHT = "The left joystick will be used to move forward, backwards, to the right and to the left. The right joystick will be used to regulate the throttle and rotation.";

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

            m_BtStart.Click += OnStartController;
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
            Controller.ControllerView cv = new Controller.ControllerView(this, m_Inverted);
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
    }
}