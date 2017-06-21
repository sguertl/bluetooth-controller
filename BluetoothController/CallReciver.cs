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
using Android.Telephony;
using Android.Util;
using Android.Provider;
using Android.Net.Sip;

namespace BluetoothController {
    public class CallReciver : BroadcastReceiver {

       
        public override void OnReceive(Context context, Intent intent) {
            String action = intent.Action;
            SipSession ss = (SipSession) new Object();
            ss.EndCall();  
        }
    }
}