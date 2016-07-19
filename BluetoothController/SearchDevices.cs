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
        // Member Variablen
        private BluetoothAdapter m_BtAdapter;
        private IntentFilter m_Filter; // Filtert Events beim Suchen
        private MyBroadcastreciver m_Receiver;
        private Button m_BtSearch;
        private LinearLayout m_Linear;
        private BluetoothDevice m_Device;
        private ListView m_ListView;
        private List<String> m_Uuids; // enthält die UUID´s
        private GradientDrawable m_Drawable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchedLayout);

            Init();
            m_BtAdapter.StartDiscovery(); // Startet die Suche nach Devices in der Nähe
        }

        public void Init()
        {

            // Erzeugen der Objekte
            m_Uuids = new List<string>();
            m_ListView = FindViewById<ListView>(Resource.Id.listViewSearched);
            m_BtSearch = FindViewById<Button>(Resource.Id.btSearchDevices);
            m_Drawable = new GradientDrawable();
            m_Linear = FindViewById<LinearLayout>(Resource.Id.linear3);
            m_BtAdapter = BluetoothAdapter.DefaultAdapter;
            m_Filter = new IntentFilter();
            m_Receiver = new MyBroadcastreciver(this);

            // Background Color [ListView]
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Black); // Setzt die Background Color auf Schwarz

            // Event beim Klicken auf ein ListViewItem 
            m_ListView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) =>
            {
                OnItemClick(sender, e);
            };

            
            // Text Color [Button]
            m_BtSearch.SetTextColor(Android.Graphics.Color.Black); // Setzt die Text Color Schwarz

            // Border
            m_Drawable.SetShape(ShapeType.Rectangle); // Formt das Drawable Objekt in ein Rechteckt
            m_Drawable.SetStroke(2, Android.Graphics.Color.Black); // Setzt die Border Stärke auf 2 und die Farbe Schwarz
            m_Drawable.SetColor(Android.Graphics.Color.White); // Setzt die Background auf Weiß
            m_BtSearch.SetBackgroundDrawable(m_Drawable); // Button übernimmt das Desgin des Drawable Objekt

            // Event beim Klicken des Buttons
            m_BtSearch.Touch += (object sender, Android.Views.View.TouchEventArgs e2) =>
            {
                if (e2.Event.Action == MotionEventActions.Down)
                    m_BtSearch.SetBackgroundColor(Android.Graphics.Color.Aquamarine); // Setzt Button Background, wenn der Button berührt wird
                else if (e2.Event.Action == MotionEventActions.Up)
                {
                    OnSearch(); // Startet die Suche nach neuen Devices in der Nähe
                    m_BtSearch.SetBackgroundDrawable(m_Drawable); // Setzt das Standart Design vom Button zurück
                }
            };


            // Activity Background
            m_Linear.SetBackgroundColor(Android.Graphics.Color.White); // Setzt die Background Color Weiß

            // Filtert nach bestimmten Events
            m_Filter.AddAction(BluetoothDevice.ActionFound); // ActionFound Event wird zum Filter hinzugefügt
            m_Filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted); // ActionDiscoveryStarted Event wird zum Filter hinzugefügt
            m_Filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished); // ActionDiscoveryFinished Event wird zum Filter hinzugefügt
            m_Filter.AddAction(BluetoothDevice.ActionUuid); // ActionUuid Event wird zum Filter hinzugefügt

            // Die hinzugefügten Events werden registriert und an das Broadcast Objekt weitergeleitet
            RegisterReceiver(m_Receiver, m_Filter);
        }

        // Gibt einen Toast, der vom Parameter (message) abhängt, aus
        public void GiveAMessage(String message)
        {
            Toast.MakeText(ApplicationContext, message, 0).Show();
        }

        // Startet die Suche nach neuen Devices in der Nähe
        public void OnSearch()
        {
            m_ListView.SetAdapter(null); // Setzt den Adapter der ListView auf Null, damit 
            m_Receiver.ResetList();
            m_BtAdapter.CancelDiscovery();
            m_BtAdapter.StartDiscovery();
            Console.ReadLine();
        }

        // Kopiert
        public void SetAdapterToListView(List<String> l)
        {
            ArrayAdapter<String> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, l);
            m_ListView.SetAdapter(adapter);
            m_ListView.SetBackgroundColor(Android.Graphics.Color.Gray);         
        }


        // kopiert
        public void OnItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            TextView view = (TextView)e.View;
            view.SetBackgroundColor(Android.Graphics.Color.Blue);
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