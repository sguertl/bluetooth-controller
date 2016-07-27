using Foundation;
using System;
using UIKit;

namespace BluetoothController.IOS
{
    public partial class PairedViewController : UIViewController
    {
        public PairedViewController (IntPtr handle) : base (handle)
        {
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