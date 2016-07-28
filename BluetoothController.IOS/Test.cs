using System;
using CoreBluetooth;
using Foundation;
using System.Timers;

namespace BluetoothController.IOS
{

	public class Test : CBCentralManagerDelegate
	{
		private CBCentralManager manager;
		public Test ()
		{
			manager = new CBCentralManager (this, null);
		}

		public override void DiscoveredPeripheral (CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
		{
            Console.WriteLine("1");
			Console.WriteLine ("Peripheral: " + peripheral.Identifier.AsString () + " UUID = " + peripheral.UUID + " Name = " + peripheral.Name);
            StopS("Methode DiscoveredPeripheral");

        }

        public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            Console.WriteLine("2");
        }

        public override void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
        {
            Console.WriteLine("3");
        }

        public override void FailedToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
        {
            Console.WriteLine("4");
        }

        public override void RetrievedConnectedPeripherals(CBCentralManager central, CBPeripheral[] peripherals)
        {
            Console.WriteLine("5");
        }


        public override void UpdatedState (CBCentralManager central)
		{
			String s = "";

			switch (central.State) {
			case CBCentralManagerState.Unknown:
				s = "Bluetooth is Unknown";
				break;
			case CBCentralManagerState.Resetting:
				s = "Bluetooth is resetting";
				break;
			case CBCentralManagerState.Unsupported:
				s = "Bluetooth is not supported";
				break;
			case CBCentralManagerState.Unauthorized:
				s = "Bluetooth is unauthorized";
				break;
			case CBCentralManagerState.PoweredOff:
				s = "Bluetooth is Off";
				break;
			case CBCentralManagerState.PoweredOn:
				s = "Bluetooth is On";
                    manager.ScanForPeripherals((CBUUID[])null);
                    var timer = new Timer(20 * 1000);
                    timer.Elapsed += (sender, e) => StopS("Timer");
                    break;
			default:
				break;
			}

			Console.WriteLine (s);
		}


        public void StopS(String d)
        {
            Console.WriteLine("From " + d);
            manager.StopScan();
            Console.WriteLine("Scan is stopped!!!");
        }

	}
}