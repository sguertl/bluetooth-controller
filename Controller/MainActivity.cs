using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Controller
{
    [Activity(Label = "Controller", MainLauncher = true, Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.UserLandscape, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(new ControllerView(this));
        }
    }   
}

