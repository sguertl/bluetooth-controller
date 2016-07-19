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

namespace Bluetooth_Verbindung
{
    [Activity(Label = "SearchDevices" , ScreenOrientation = Android.Content.PM.ScreenOrientation.UserLandscape)]
    public class SearchDevices : Activity
   {
        private BluetoothAdapter btAdapter;
        private IntentFilter filter;
        private MyBroadcastreciver receiver;
        private Button btSearch;
        private LinearLayout linear;
        private ListView listView;
        private List<String> uuids;
        private BluetoothDevice m_Device;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SearchLayout);
            

            init();
        }

        // Kopiert
        public void init()
        {
            uuids = new List<string>();

            listView = FindViewById<ListView>(Resource.Id.listViewSearched);
            listView.SetBackgroundColor(Android.Graphics.Color.Black);

            listView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) =>
            {
                onItemClick(sender, e);
            };

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

            receiver = new MyBroadcastreciver(this);
            filter.AddAction(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            filter.AddAction(BluetoothDevice.ActionUuid);

            RegisterReceiver(receiver, filter);

            /*
            filter = new IntentFilter(BluetoothAdapter.ActionDiscoveryStarted);
            RegisterReceiver(receiver, filter);
            filter = new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished);
            RegisterReceiver(receiver, filter);
            filter = new IntentFilter(BluetoothAdapter.ActionStateChanged);
            RegisterReceiver(receiver, filter);
            */
        }

        //Kopiert
        public void GiveAMessage(String s)
        {
            Toast.MakeText(ApplicationContext, s, 0).Show();
        }

        //Kopiert
        public void onSearch()
        {
            listView.SetAdapter(null);
            receiver.setListNeu();
            btAdapter.CancelDiscovery();
            btAdapter.StartDiscovery();
            Console.ReadLine();
        }

        // Kopiert
        public void setAdapterToListView(List<String> l)
        {
            ArrayAdapter<String> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, l);
            //listView.Adapter = adapter;
            listView.SetAdapter(adapter);
        }


        // kopiert
        public void onItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            TextView view = (TextView)e.View;
            String address = view.Text.Split('\n')[1];
            BluetoothDevice btDevice = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
            m_Device = btDevice;
            for (int i = 0; i < uuids.Count; i++)
            {
                Console.WriteLine("For Schleife |||||||||||||||||||||||||" + uuids.ElementAt(i));
            }

            Console.WriteLine("////////////////  "+ "Device : " + btDevice.Name +"  "+uuids[e.Position]);
            ConnectedThread connect = new ConnectedThread(btDevice, uuids[e.Position]);
            Console.ReadLine();
            connect.Start();

            var activity2 = new Intent(this, typeof(ConnectedDevices));
            IList<String> ll = new List<string>();
            ll.Add(m_Device.Name);
            ll.Add(m_Device.Address);
            activity2.PutStringArrayListExtra("MyData", ll);
            StartActivity(activity2);


            // StartActivity(typeof(ConnectedDevices));

            //  Toast.MakeText(ApplicationContext, btDevice.GetUuids(), 0).Show();

        }

        // Kopiert
        public void AddUUid(String uuid)
        {
            uuids.Add(uuid);
        }

    }
}