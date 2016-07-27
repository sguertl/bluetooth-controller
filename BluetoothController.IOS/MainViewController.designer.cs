// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace BluetoothController.IOS
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPairedView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSearchView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblHeading { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView mainView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnPairedView != null) {
                btnPairedView.Dispose ();
                btnPairedView = null;
            }

            if (btnSearchView != null) {
                btnSearchView.Dispose ();
                btnSearchView = null;
            }

            if (lblHeading != null) {
                lblHeading.Dispose ();
                lblHeading = null;
            }

            if (mainView != null) {
                mainView.Dispose ();
                mainView = null;
            }
        }
    }
}