using CoreBluetooth;
using CoreFoundation;
using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExternalAccessory;

namespace BluetoothController.IOS
{
    public partial class SearchViewController : UIViewController
    {
        private MyCBCentralManagerDelegate myManagerDelegate;
		private List<CBPeripheral> mDiscoveredDevices;
		private CBCentralManager mCentralManager;
		public event EventHandler ScanTimeoutElapsed = delegate { };
		public event EventHandler<CBDiscoveredPeripheralEventArgs> DeviceDiscovered = delegate { };
		public event EventHandler<CBPeripheralEventArgs> DeviceConnected = delegate { };
		public event EventHandler<CBPeripheralErrorEventArgs> DeviceDisconnected = delegate { };

        public SearchViewController (IntPtr handle) : base (handle)
        {
			mCentralManager = new CBCentralManager (DispatchQueue.CurrentQueue);

			mDiscoveredDevices = new List<CBPeripheral> ();
			mCentralManager.DiscoveredPeripheral += (object sender, CBDiscoveredPeripheralEventArgs e) => {
				Console.WriteLine ("Discovered Peripheral " + e.Peripheral.Identifier);
				mDiscoveredDevices.Add (e.Peripheral);
				DeviceDiscovered (this, e);
			};

			// 55BC208A-5FB5-5619-FDF9-106345844366
			// 55BC208A-5FB5-5619-FDF9-106345844366

			mCentralManager.UpdatedState += (object sender, EventArgs e) => {
				Console.WriteLine ("UpdatedState: " + mCentralManager.State);
			};

			mCentralManager.ConnectedPeripheral += (object sender, CBPeripheralEventArgs e) => {
				Console.WriteLine ("Connected Peripheral: " + e.Peripheral.Name);
				if (!mDiscoveredDevices.Contains (e.Peripheral)) {
					mDiscoveredDevices.Add (e.Peripheral);
					mCentralManager.ConnectPeripheral (e.Peripheral, (NSDictionary) null);
				}
				DeviceConnected (sender, e);
			};

			mCentralManager.DisconnectedPeripheral += (object sender, CBPeripheralErrorEventArgs e) => {
				Console.WriteLine ("Disconnected Peripheral: " + e.Peripheral.Name);
				if (mDiscoveredDevices.Contains (e.Peripheral)) {
					mDiscoveredDevices.Remove (e.Peripheral);
				}
				DeviceDisconnected (sender, e);
			};

			isScanning = false;
        }

		public bool isScanning { get; set; }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			btnSearch.Layer.CornerRadius = 5;
			btnSearch.Layer.MasksToBounds = true;

			btnBackToMain.Layer.CornerRadius = 5;
			btnBackToMain.Layer.MasksToBounds = true;
            btnSearch.TouchUpInside += OnSearchDevices;
		}

        private void OnSearchDevices(object sender, EventArgs args)
        {
			BeginScanningForDevices ();
		}

		public async Task BeginScanningForDevices ()
		{
			Console.WriteLine ("Begin scanning");
			mDiscoveredDevices.Clear ();
			isScanning = true;
			//mCentralManager.ScanForPeripherals (peripheralUuids: null);
			mCentralManager.ScanForPeripherals ((CBUUID [])null);
			//mCentralManager.ScanForPeripherals (new[] { UUID });

			await Task.Delay (10000);

			if (isScanning) {
				mCentralManager.StopScan ();
				ScanTimeoutElapsed (this, new EventArgs ());
			}
		}

		public void StopScanningForDevices ()
		{
			Console.WriteLine ("stop scanning");
			isScanning = false;
			mCentralManager.StopScan ();
		}

		public void DisconnectPeripheral (CBPeripheral peripheral)
		{
			mCentralManager.CancelPeripheralConnection (peripheral);
		}

		public override bool ShouldAutorotate ()
		{
			return false;
		}

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation ()
		{
			return UIInterfaceOrientation.Portrait;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait;
		}
    }
}