using Foundation;
using System;
using UIKit;

namespace BluetoothController.IOS
{
    public partial class SearchViewController : UIViewController
    {
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
		}


    }
}