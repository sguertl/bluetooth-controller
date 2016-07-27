using System;
using Foundation;
using UIKit;

namespace BluetoothController.IOS
{
	public partial class MainViewController : UIViewController
	{
		protected MainViewController (IntPtr handle) : base (handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			btnSearchView.Layer.CornerRadius = 5;
			btnSearchView.Layer.MasksToBounds = true;

			btnPairedView.Layer.CornerRadius = 5;
			btnPairedView.Layer.MasksToBounds = true;
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
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

		/*
		public override void PrepareForSegue (UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue (segue, sender);

			switch (segue.Identifier) 
			{
				case "SegueToSearch": break;
			}
		}

		public override bool ShouldPerformSegue (string segueIdentifier, Foundation.NSObject sender)
		{


			return true;
		}*/
	}
}

