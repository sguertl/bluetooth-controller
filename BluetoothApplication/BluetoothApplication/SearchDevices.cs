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
    /// <summary>
    /// Searches and shows all Bluetooth devices close by
    /// </summary>
    [Activity(Label = "SearchDevices")]
    public class SearchDevices : Activity
    {
        // Member Variablen
        private BluetoothAdapter m_BluetoothAdapter;
        private MySearchBroadcastReceiver m_Searchreceiver;
        private MyPairBroadcastReceiver m_Pairreceiver;
        private Button m_BtSearch;
        private LinearLayout m_LinearLayout;
        private ListView m_ListView;
        private ProgressDialog m_ProgressDialog;
        private List<String> m_Uuids;
        //

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchLayout);
            init();
        }

        /// <summary>
        /// Click event for ListViewItem
        /// </summary>
        private void OnClickListView(Object sender, AdapterView.ItemClickEventArgs e)
        {
            TextView view = (TextView)e.View;
            String address = view.Text.Split('\n')[1];
            BluetoothDevice btDevice = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address);
            for (int i = 0; i < m_Uuids.Count; i++)
            {
                Console.WriteLine("For Schleife |||||||||||||||||||||||||" + m_Uuids.ElementAt(i));
            }

            Console.WriteLine("////////////////  " + "Device : " + btDevice.Name + "  " + m_Uuids[e.Position]);
            ConnectedThread connect = new ConnectedThread(btDevice, m_Uuids[e.Position]);
            Console.ReadLine();
            connect.Start();
            // PairDevice(device);
        }

        /// <summary>
        /// Pairing Device
        /// </summary>
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

        /// <summary>
        /// Intializes members and makes the style for ui
        /// </summary>
        private void init()
        {
            m_Uuids = new List<string>();

            m_ListView = FindViewById<ListView>(Resource.Id.listViewSearched);
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black);
            m_ListView.ItemClick += OnClickListView;

            m_BtSearch = FindViewById<Button>(Resource.Id.btSearchDevices);

            m_BtSearch.SetTextColor(Android.Graphics.Color.Black);

            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke(2, Android.Graphics.Color.Black);

            drawable.SetColor(Android.Graphics.Color.White);
            m_BtSearch.SetBackgroundDrawable(drawable);

            m_BtSearch.Click += delegate
            {
                onSearch();
            };

            m_LinearLayout = FindViewById<LinearLayout>(Resource.Id.linear3);

            m_LinearLayout.SetBackgroundColor(Android.Graphics.Color.White);

            m_BluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            m_ProgressDialog = new ProgressDialog(this);
            m_ProgressDialog.SetMessage("Scanning for Devices...");
            m_ProgressDialog.SetCancelable(false);
            m_ProgressDialog.CancelEvent += delegate { m_ProgressDialog.Dismiss(); m_BluetoothAdapter.CancelDiscovery(); };

            m_Searchreceiver = new MySearchBroadcastReceiver(this);
            IntentFilter filter = new IntentFilter();
            filter.AddAction(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            filter.AddAction(BluetoothDevice.ActionUuid);

            RegisterReceiver(m_Searchreceiver, filter);

            m_Pairreceiver = new MyPairBroadcastReceiver(this);
            IntentFilter pairFilter = new IntentFilter(BluetoothDevice.ActionBondStateChanged);
            RegisterReceiver(m_Pairreceiver, pairFilter);
        }

        /// <summary>
        /// Prints message in Toast
        /// </summary>
        public void GiveAMessage(String s)
        {
            Toast.MakeText(ApplicationContext, s, 0).Show();
        }

        /// <summary>
        /// Sucht nach Devices
        /// </summary>
        private void onSearch()
        {
            m_ListView.SetAdapter(null);
            m_Searchreceiver.setListNeu();
            m_BluetoothAdapter.CancelDiscovery();
            m_ProgressDialog.Show();
            m_BluetoothAdapter.StartDiscovery();
        }

        public void setAdapterToListView(List<String> l)
        {
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, l);
            m_ListView.Adapter = adapter;
            m_ProgressDialog.Dismiss();
        }
        public void AddUUid(String uuid)
        {
            m_Uuids.Add(uuid);
        }

    }
}