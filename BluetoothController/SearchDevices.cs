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

namespace BluetoothController
{

    // --------------- DOKUMENTATION --------------------

    [Activity(Label = "SearchedDevices")]
    public class SearchDevices : Activity
    {
        private BluetoothAdapter m_BtAdapter;
        private IntentFilter m_Filter;
        private MyBroadcastreciver m_Receiver;
        private Button m_BtSearch;
        private LinearLayout m_Linear;
        private BluetoothDevice m_Device;
        private ListView m_ListView;
        private List<String> m_Uuids;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchedLayout);

            Init();
        }

        public void Init()
        {
            m_Uuids = new List<string>();

            m_ListView = FindViewById<ListView>(Resource.Id.listViewSearched);
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black);

            m_ListView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) =>
            {
                OnItemClick(sender, e);
            };

            m_BtSearch = FindViewById<Button>(Resource.Id.btSearchDevices);

            m_BtSearch.SetTextColor(Android.Graphics.Color.Black);

            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke(2, Android.Graphics.Color.Black);

            drawable.SetColor(Android.Graphics.Color.White);
            m_BtSearch.SetBackgroundDrawable(drawable);

            m_BtSearch.Click += delegate
            {
                OnSearch();
            };

            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear3);

            m_Linear.SetBackgroundColor(Android.Graphics.Color.White);

            m_BtAdapter = BluetoothAdapter.DefaultAdapter;

            m_Filter = new IntentFilter();
            m_Receiver = new MyBroadcastreciver(this);

            m_Filter.AddAction(BluetoothDevice.ActionFound);
            m_Filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
            m_Filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            m_Filter.AddAction(BluetoothDevice.ActionUuid);

            RegisterReceiver(m_Receiver, m_Filter);
        }

        // Gibt einen Toast, der vom Parameter  abhängt, aus
        public void GiveAMessage(String message)
        {
            Toast.MakeText(ApplicationContext, message, 0).Show();
        }

        // OnSearch
        public void OnSearch()
        {
            m_ListView.SetAdapter(null);
            m_Receiver.ResetList();
            m_BtAdapter.CancelDiscovery();
            m_BtAdapter.StartDiscovery();
            Console.ReadLine();
        }

        // Kopiert
        public void SetAdapterToListView(List<String> l)
        {
            ArrayAdapter<String> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, l);
            //listView.Adapter = adapter;
            m_ListView.SetAdapter(adapter);
        }


        // kopiert
        public void OnItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            TextView view = (TextView)e.View;
            String address = view.Text.Split('\n')[1];
            BluetoothDevice btDevice = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
            m_Device = btDevice;

            ConnectedThread connect = new ConnectedThread(btDevice, m_Uuids[e.Position]);
            connect.Start();

            var activity2 = new Intent(this, typeof(ConnectedDevices));
            IList<String> ll = new List<string>();
            ll.Add(m_Device.Name);
            ll.Add(m_Device.Address);
            activity2.PutStringArrayListExtra("MyData", ll);
            StartActivity(activity2);
        }

        public void AddUuid(String uuid)
        {
            m_Uuids.Add(uuid);
        }

    }
}