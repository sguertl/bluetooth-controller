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

		public ControllerView ()
		{
			CGRect screenSize = UIScreen.MainScreen.Bounds;
			SCREEN_WIDTH = (float)screenSize.Width;
			SCREEN_HEIGHT = (float)screenSize.Height;

			InitJoysticks ();
		}

		private void InitJoysticks ()
		{
			m_LeftJS = new Joystick (SCREEN_WIDTH, SCREEN_HEIGHT, true, false);
			m_RightJS = new Joystick (SCREEN_WIDTH, SCREEN_HEIGHT, false, false);

			m_CircleJSLeft = new CGRect (m_LeftJS.CenterX - Joystick.STICK_RADIUS,
			                             m_LeftJS.CenterY - Joystick.STICK_RADIUS,
			                             Joystick.STICK_RADIUS * 2, Joystick.STICK_RADIUS * 2);
			
			m_CircleJSRight = new CGRect (m_RightJS.CenterX - Joystick.STICK_RADIUS,
			                             m_RightJS.CenterY - Joystick.STICK_RADIUS,
										 Joystick.STICK_RADIUS * 2, Joystick.STICK_RADIUS * 2);


		}

		public override void Draw (CoreGraphics.CGRect rect)
		{
			base.Draw (rect);

			using (CGContext g = UIGraphics.GetCurrentContext ()) {

				//set up drawing attributes




				g.SetLineWidth (10);
				UIColor.Blue.SetFill ();
				UIColor.Red.SetStroke ();

				g.SetFillColor (UIColor.Red.CGColor);

				g.AddEllipseInRect (m_CircleJSLeft);
				g.AddEllipseInRect (m_CircleJSRight);

				//create geometry
				var path = new CGPath ();

				path.AddLines (new CGPoint []{
					new CGPoint (100, 200),
					new CGPoint (160, 100),
					new CGPoint (220, 200)});

				path.CloseSubpath ();

				//add geometry to graphics context and draw it
				g.AddPath (path);
				g.DrawPath (CGPathDrawingMode.FillStroke);
			}
		}
	}
}

