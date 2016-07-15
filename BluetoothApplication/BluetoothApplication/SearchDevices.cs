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
using Android.Bluetooth;
using Android.Graphics.Drawables;
using Java.Lang.Reflect;

namespace BluetoothApplication
{
    [Activity(Label = "SearchDevices")]
    public class SearchDevices : Activity
    {
        private BluetoothAdapter btAdapter;
        private IntentFilter filter;
        private MyBroadcastReceiver receiver;
        private MyPairBroadcastReceiver pairreceiver;
        private Button btSearch;
        private LinearLayout linear;
        private ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchLayout);

            init();
        }

        private void OnClickListView(Object sender, AdapterView.ItemClickEventArgs e)
        {
            TextView view = (TextView)e.View;
            String address = view.Text.Split('\n')[1];
            BluetoothDevice device = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
            PairDevice(device);
        }

        private void PairDevice(BluetoothDevice device)
        {
            try
            {
                Method method = device.Class.GetMethod("createBond", (Java.Lang.Class[])null);
                method.Invoke(device, (Java.Lang.Object[])null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class MyPairBroadcastReceiver : BroadcastReceiver
        {
            // Bond_bonded = 12
            // Bond_bonding = 11
            // Bond_none = 10
            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;
                if(BluetoothDevice.ActionBondStateChanged == action)
                {
                    int state = intent.GetIntExtra(BluetoothDevice.ExtraBondState, BluetoothDevice.Error);
                    int prevstate = intent.GetIntExtra(BluetoothDevice.ExtraPreviousBondState, BluetoothDevice.Error);

                    if(state == 12 && prevstate == 11)
                    {
                        Console.WriteLine("Paired");
                    }
                    else if(state == 10 && prevstate == 12)
                    {
                        Console.WriteLine("Unpaired");
                    }
                }
            }
        }

        public void init()
        {
            listView = FindViewById<ListView>(Resource.Id.listViewSearched);
            listView.SetBackgroundColor(Android.Graphics.Color.Black);
            listView.ItemClick += OnClickListView;

            btSearch = FindViewById<Button>(Resource.Id.btSearchDevices);

            btSearch.SetTextColor(Android.Graphics.Color.Black);

            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke(2, Android.Graphics.Color.Black);

            drawable.SetColor(Android.Graphics.Color.White);
            btSearch.SetBackgroundDrawable(drawable);

            btSearch.Click += delegate
            {
                onSearch();
            };

            linear = FindViewById<LinearLayout>(Resource.Id.linear3);

            linear.SetBackgroundColor(Android.Graphics.Color.White);

            btAdapter = BluetoothAdapter.DefaultAdapter;

            filter = new IntentFilter();

            // filter = new IntentFilter(BluetoothDevice.ActionFound);

            receiver = new MyBroadcastReceiver(this);
            filter.AddAction(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);

            RegisterReceiver(receiver, filter);

            pairreceiver = new MyPairBroadcastReceiver();
            IntentFilter pairFilter = new IntentFilter(BluetoothDevice.ActionBondStateChanged);
            RegisterReceiver(pairreceiver, pairFilter);
        }

        public void GiveAMessage(String s)
        {
            Toast.MakeText(ApplicationContext, s, 0).Show();
        }

        public void onSearch()
        {
            btAdapter.CancelDiscovery();
            btAdapter.StartDiscovery();
        }

        public void setAdapterToListView(List<String> l)
        {
            ArrayAdapter<String> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, l);
            //listView.Adapter = adapter;
            listView.SetAdapter(adapter);
        }
    }
}