using CoreBluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Foundation;

namespace BluetoothController.IOS
{
    public class MyCBCentralManagerDelegate : CBCentralManagerDelegate
    {
        public override void UpdatedState(CBCentralManager manager)
        {
            if(manager.State == CBCentralManagerState.PoweredOn)
            {
                CBUUID[] cbuuids = null;
                manager.ScanForPeripherals(cbuuids);
                var timer = new Timer(30000);
                timer.Elapsed += (sender, e) => manager.StopScan();
            }
            else
            {
                Console.WriteLine("Bluetooth is not available");
            }   
        }

        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
        {
            Console.WriteLine("Discovered {0}, data {1}, RSSI {2}", peripheral.Name, advertisementData, RSSI);
        }
    }
}
