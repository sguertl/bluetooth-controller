using CoreBluetooth;
using CoreFoundation;
using Foundation;
using System;
using UIKit;

namespace BluetoothController.IOS
{
    public partial class SearchViewController : UIViewController
    {
        private MyCBCentralManagerDelegate myManagerDelegate;

        public SearchViewController (IntPtr handle) : base (handle)
        {
			
        }

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
            myManagerDelegate = new MyCBCentralManagerDelegate();
            var managerDelegate = new CBCentralManager(myManagerDelegate, DispatchQueue.CurrentQueue);
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