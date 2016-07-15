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

namespace BluetoothApplication
{
    /// <summary>
    /// BroadcastReceiver for Pairing with devices
    /// </summary>
    public class MyPairBroadcastReceiver : BroadcastReceiver
    {
        private readonly int BOND_BONDED = 12;
        private readonly int BOND_BONDING = 11;
        private readonly int BOND_NONE = 10;
        private SearchDevices mSearchDevices;

        public MyPairBroadcastReceiver(SearchDevices searchDevices)
        {
            mSearchDevices = searchDevices;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;
            if (BluetoothDevice.ActionBondStateChanged == action)
            {
                int state = intent.GetIntExtra(BluetoothDevice.ExtraBondState, BluetoothDevice.Error);
                int prevstate = intent.GetIntExtra(BluetoothDevice.ExtraPreviousBondState, BluetoothDevice.Error);

                if (state == 12 && prevstate == 11)
                {
                    mSearchDevices.GiveAMessage("Paired");
                }
                else if (state == 10 && prevstate == 12)
                {
                    mSearchDevices.GiveAMessage("Unpaired");
                }
            }
        }
    }
}