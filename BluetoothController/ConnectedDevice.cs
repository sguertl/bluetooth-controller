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

    [Activity(Label = "ConnectedDevice" ,Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class ConnectedDevices : Activity
    {
        // Members
        private TextView m_ViewName; // TextView for displaying the device's name
        private TextView m_ViewAddress; // TextView for displaying the device's address
        private Button m_BtControl; // Button for controlling the device
        private Button m_BtDisconnect; // Button for disconnecting from the device
        private LinearLayout m_Linear;
        private bool m_Outside = false;
        private GradientDrawable m_Drawable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ConnectedDevicesLayout);

            Init();
        }

        /// <summary>
        /// Initializes and modifies the members
        /// </summary>
        public void Init()
        {
            // Initializing objects
            m_ViewName = FindViewById<TextView>(Resource.Id.DeviceName);
            m_ViewAddress = FindViewById<TextView>(Resource.Id.DeviceAdresse);
            m_BtControl = FindViewById<Button>(Resource.Id.btSteueren);
            m_BtDisconnect = FindViewById<Button>(Resource.Id.btDisconnect);
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear4);

            // Setting text color of buttons
            m_BtControl.SetTextColor(Android.Graphics.Color.Black);
            m_BtDisconnect.SetTextColor(Android.Graphics.Color.Black);

            // Setting border of buttons
            m_Drawable = new GradientDrawable();
            m_Drawable.SetShape(ShapeType.Rectangle);
            m_Drawable.SetStroke(2, Android.Graphics.Color.Black);
            m_Drawable.SetColor(Android.Graphics.Color.White);
            m_BtControl.Background = m_Drawable;
            m_BtDisconnect.Background = m_Drawable;

            // Setting activity background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);

            // Receiving data from other activities
            IList<String> text = Intent.GetStringArrayListExtra("MyData");
            m_ViewName.Text = text.ElementAt(0);
            m_ViewAddress.Text = text.ElementAt(1);

            // Setting text color of textviews
            m_ViewName.SetTextColor(Android.Graphics.Color.Black);
            m_ViewAddress.SetTextColor(Android.Graphics.Color.Black);

            // Handling button contact
            m_BtControl.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                // Changing button background if button was touched
                if (e2.Event.Action == MotionEventActions.Down)
                {
                    m_BtControl.SetBackgroundColor(Android.Graphics.Color.Aquamarine);
                }
                // Starting a new activity and setting default button background if
                // button was released
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    if (!m_Outside)
                    {
                        StartActivity(typeof(ControllerActivity));
                    }
                    //m_BtControl.SetBackgroundDrawable(drawable);
                    m_BtControl.Background = m_Drawable;
                }else if(e2.Event.Action == MotionEventActions.Move)
                {
                    if (e2.Event.GetY() + m_BtControl.GetY() >= m_BtControl.Top && e2.Event.GetY() + m_BtControl.GetY() <= m_BtControl.Bottom &&
                   e2.Event.GetX() >= m_BtControl.Left && e2.Event.GetX() <= m_BtControl.Right)
                    {
                        m_Outside = false;
                        m_BtControl.SetBackgroundColor(Android.Graphics.Color.Aqua);
                    }
                    else
                    {
                        m_Outside = true;
                        m_BtControl.Background = m_Drawable;
                    }
                }
            };
        }
    }
}