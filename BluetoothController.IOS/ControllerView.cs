using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Drawing;

namespace BluetoothController.IOS
{
	public partial class ControllerView : UIView
	{

		private readonly float SCREEN_WIDTH;
		private readonly float SCREEN_HEIGHT;

		private Joystick m_LeftJS;
		private Joystick m_RightJS;

		private CGRect m_CircleJSLeft;
		private CGRect m_CircleJSRight;
		private CGRect m_CircleDPLeft;
		private CGRect m_CircleDPRight;

		public ControllerView (IntPtr p) : base (p) { }

		public ControllerView ()
		{
			CGRect screenSize = UIScreen.MainScreen.Bounds;
			SCREEN_WIDTH = (float)screenSize.Width;
			SCREEN_HEIGHT = (float)screenSize.Height;

			UserInteractionEnabled = true;
			MultipleTouchEnabled = true;

			Console.WriteLine ("Screenwidth: " + SCREEN_WIDTH + ", Screenheight: " + SCREEN_HEIGHT);

			InitJoysticks ();
		}

		private void InitJoysticks ()
		{
			Console.WriteLine ("Init() called");
			m_LeftJS = new Joystick (SCREEN_WIDTH, SCREEN_HEIGHT, true, false);
			m_RightJS = new Joystick (SCREEN_WIDTH, SCREEN_HEIGHT, false, false);

			m_CircleJSLeft = new CGRect (m_LeftJS.CenterX - Joystick.STICK_RADIUS,
			                             m_LeftJS.CenterY - Joystick.STICK_RADIUS,
			                             Joystick.STICK_RADIUS * 2, Joystick.STICK_RADIUS * 2);

			m_CircleJSRight = new CGRect (m_RightJS.CenterX - Joystick.STICK_RADIUS,
			                              m_RightJS.CenterY - Joystick.STICK_RADIUS,
										  Joystick.STICK_RADIUS * 2, Joystick.STICK_RADIUS * 2);

			m_CircleDPLeft = new CGRect (m_LeftJS.CenterX - Joystick.DISPLACEMENT_RADIUS,
										 m_LeftJS.CenterY - Joystick.DISPLACEMENT_RADIUS,
										 Joystick.DISPLACEMENT_RADIUS * 2, Joystick.DISPLACEMENT_RADIUS * 2);

			m_CircleDPRight = new CGRect (m_RightJS.CenterX - Joystick.DISPLACEMENT_RADIUS,
										  m_RightJS.CenterY - Joystick.DISPLACEMENT_RADIUS,
										  Joystick.DISPLACEMENT_RADIUS * 2, Joystick.DISPLACEMENT_RADIUS * 2);
			
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			SetNeedsDisplay ();
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
		}
	}
}

