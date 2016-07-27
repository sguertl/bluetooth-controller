using CoreBluetooth;
using CoreFoundation;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
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
            mDiscoveredDevices = new List<CBPeripheral>();
            mCentralManager.DiscoveredPeripheral += (object sender, CBDiscoveredPeripheralEventArgs e) =>
            {
                Console.WriteLine("Discovered Peripherel " + e.Peripheral.Name);
                mDiscoveredDevices.Add(e.Peripheral);
                DeviceDiscovered(this, e);
            };

            mCentralManager.UpdatedState += (object sender, EventArgs e) =>
            {
                Console.WriteLine("UpdatedState: " + mCentralManager.State);
            };

            mCentralManager.ConnectedPeripheral += (object sender, CBPeripheralEventArgs e) =>
            {
                Console.WriteLine("Connected Peripheral: " + e.Peripheral.Name);
                if (!mDiscoveredDevices.Contains(e.Peripheral))
                {
                    mDiscoveredDevices.Add(e.Peripheral);
                }
                DeviceConnected(sender, e);
            };

            mCentralManager.DisconnectedPeripheral += (object sender, CBPeripheralErrorEventArgs e) =>
            {
                Console.WriteLine("Disconnected Peripheral: " + e.Peripheral.Name);
                if (mDiscoveredDevices.Contains(e.Peripheral))
                {
                    mDiscoveredDevices.Remove(e.Peripheral);
                }
                DeviceDisconnected(sender, e);
            };

            isScanning = false;
            mCentralManager = new CBCentralManager(DispatchQueue.CurrentQueue);
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
            BeginScanningForDevices();
        }

        public async Task BeginScanningForDevices()
        {
            Console.WriteLine("Beginn scanning");
            mDiscoveredDevices.Clear();
            isScanning = true;
            mCentralManager.ScanForPeripherals(peripheralUuids: null);

            await Task.Delay(10000);

            if (isScanning)
            {
                mCentralManager.StopScan();
                ScanTimeoutElapsed(this, new EventArgs());
            }
        }

        public void StopScanningForDevices()
        {
            Console.WriteLine("stop scanning");
            isScanning = false;
            mCentralManager.StopScan();
        }

        public void DisconnectPeripheral(CBPeripheral peripheral)
        {
            mCentralManager.CancelPeripheralConnection(peripheral);
            myManagerDelegate = new MyCBCentralManagerDelegate();
            var managerDelegate = new CBCentralManager(myManagerDelegate, DispatchQueue.CurrentQueue);
            managerDelegate.ScanForPeripherals((CBUUID[])null);

			/*CBCentralManager manager = new CBCentralManager ();
			manager.ScanForPeripherals (CBUUID.FromString("713D0003-503E-4C75-BA94-3148F18D941E"));
			manager.DiscoveredPeripheral += delegate {
				Console.WriteLine ("Discovered");
			};*/
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