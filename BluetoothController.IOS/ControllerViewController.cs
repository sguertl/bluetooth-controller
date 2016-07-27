using Foundation;
using System;
using UIKit;

namespace BluetoothController.IOS
{
    public partial class ControllerViewController : UIViewController
    {
        public ControllerViewController (IntPtr handle) : base (handle)
        {
			
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.AddSubview (controllerView);
		}

		public override bool ShouldAutorotate ()
		{
			return true;
		}

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation ()
		{
			return UIInterfaceOrientation.LandscapeLeft;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Landscape;
		}
    }
}