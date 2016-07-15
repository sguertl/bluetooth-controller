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
        private BluetoothAdapter mBluetoothAdapter;
        private MySearchBroadcastReceiver mSearchreceiver;
        private MyPairBroadcastReceiver mPairreceiver;
        private Button btSearch;
        private LinearLayout linearLayout;
        private ListView listView;
        private ProgressDialog mProgressDialog;
        private List<String> mUuids;

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
            for (int i = 0; i < mUuids.Count; i++)
            {
                Console.WriteLine("For Schleife |||||||||||||||||||||||||" + mUuids.ElementAt(i));
            }

            Console.WriteLine("////////////////  " + "Device : " + btDevice.Name + "  " + mUuids[e.Position]);
            ConnectedThread connect = new ConnectedThread(btDevice, mUuids[e.Position]);
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
            mUuids = new List<string>();

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

            linearLayout = FindViewById<LinearLayout>(Resource.Id.linear3);

            linearLayout.SetBackgroundColor(Android.Graphics.Color.White);

            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            mProgressDialog = new ProgressDialog(this);
            mProgressDialog.SetMessage("Scanning for Devices...");
            mProgressDialog.SetCancelable(false);
            mProgressDialog.CancelEvent += delegate { mProgressDialog.Dismiss(); mBluetoothAdapter.CancelDiscovery(); };

            mSearchreceiver = new MySearchBroadcastReceiver(this);
            IntentFilter filter = new IntentFilter();
            filter.AddAction(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            filter.AddAction(BluetoothDevice.ActionUuid);

            RegisterReceiver(mSearchreceiver, filter);

            mPairreceiver = new MyPairBroadcastReceiver(this);
            IntentFilter pairFilter = new IntentFilter(BluetoothDevice.ActionBondStateChanged);
            RegisterReceiver(mPairreceiver, pairFilter);
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
            listView.SetAdapter(null);
            mSearchreceiver.setListNeu();
            mBluetoothAdapter.CancelDiscovery();
            mProgressDialog.Show();
            mBluetoothAdapter.StartDiscovery();
        }

        public void setAdapterToListView(List<String> l)
        {
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, l);
            listView.Adapter = adapter;
            mProgressDialog.Dismiss();
        }
        public void AddUUid(String uuid)
        {
            mUuids.Add(uuid);
        }

    }
}