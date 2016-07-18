using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Graphics.Drawables;

namespace BluetoothApplication
{
    [Activity(Label = "BluetoothApplication", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // Member Variablen
        private BluetoothAdapter m_BluetoothAdapter;
        private Button m_BtPairedDevices;
        private Button m_BtSearchDevices;
        //

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Init();

            //Check wether the device supports Bluetooth
            if (m_BluetoothAdapter == null)
            {
                Toast.MakeText(ApplicationContext, "Bluetooth is not supported", 0).Show();
                AlertDialog alertMessage = new AlertDialog.Builder(this).Create();
                alertMessage.SetTitle("Bluetooth not supported");
                alertMessage.SetMessage("Bluetooth is not supported on this device. Please ... ");
                alertMessage.SetButton("Ok", (s, ev) => { Finish(); });
                alertMessage.Show();
                m_BtPairedDevices.Enabled = false;
                m_BtSearchDevices.Enabled = false;
            }
            else
            {
                if (!m_BluetoothAdapter.IsEnabled)
                {
                    turnBluetoothOn();
                }
            }
        }

        /// <summary>
        /// Intializes members and makes the style for ui
        /// </summary>
        private void Init()
        {
            m_BluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            m_BtPairedDevices = FindViewById<Button>(Resource.Id.btPairedDevices);
            m_BtSearchDevices = FindViewById<Button>(Resource.Id.btSearchDevices);

            // Text Color
            m_BtPairedDevices.SetTextColor(Android.Graphics.Color.Black);
            m_BtSearchDevices.SetTextColor(Android.Graphics.Color.Black);

            // Border
            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke(2, Android.Graphics.Color.Black);

            drawable.SetColor(Android.Graphics.Color.White);
            m_BtPairedDevices.SetBackgroundDrawable(drawable);
            m_BtSearchDevices.SetBackgroundDrawable(drawable);

            // Main Background
            LinearLayout line = FindViewById<LinearLayout>(Resource.Id.linear);
            line.SetBackgroundColor(Android.Graphics.Color.White);

            // Add mouse Clicked
            m_BtPairedDevices.Click += delegate
            {
                StartActivity(typeof(PairedDevices));
            };

            m_BtSearchDevices.Click += delegate
            {
                StartActivity(typeof(SearchDevices));
            };
        }

        private void turnBluetoothOn()
        {
            Intent intent = new Intent(BluetoothAdapter.ActionRequestEnable);
            StartActivityForResult(intent, 1);
        }
    }
}

