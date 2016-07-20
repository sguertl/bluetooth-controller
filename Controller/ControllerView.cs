using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Graphics.Drawables.Shapes;

namespace Controller
{
    public class ControllerView : View, View.IOnTouchListener
    {
        // Screen metrics in px
        public readonly float SCREEN_WIDTH;
        public readonly float SCREEN_HEIGHT;

        // Joystick ovals
        private ShapeDrawable m_ShapeStickLeft;
        private ShapeDrawable m_ShapeStickRight;

        // Displacement ovals
        private ShapeDrawable m_ShapeRadiusLeft;
        private ShapeDrawable m_ShapeRadiusRight;

        // Joystick controllers
        private Joystick m_LeftJS;
        private Joystick m_RightJS;

        public ControllerView(Context context) : base(context)
        {
            SetOnTouchListener(this);
            SetBackgroundColor(Color.White);

            SCREEN_WIDTH = Resources.DisplayMetrics.WidthPixels; //ConvertPixelsToDp(metrics.WidthPixels);
            SCREEN_HEIGHT = Resources.DisplayMetrics.HeightPixels; //ConvertPixelsToDp(metrics.HeightPixels);

            InitShapes();
            InitJoysticks();
        }

        /// <summary>
        /// Initializes the joystick and displacement shapes
        /// </summary>
        private void InitShapes()
        {
            // Paint for joystick ovals
            var paintStick = new Paint();
            paintStick.SetARGB(255, 78, 78, 78);
            paintStick.SetStyle(Paint.Style.Fill);
            // Shape for left joystick
            m_ShapeStickLeft = new ShapeDrawable(new OvalShape());
            m_ShapeStickLeft.Paint.Set(paintStick);
            // Shape for right joystick
            m_ShapeStickRight = new ShapeDrawable(new OvalShape());
            m_ShapeStickRight.Paint.Set(paintStick);

            // Paint for displacement ovals
            var paintRadius = new Paint();
            paintRadius.SetARGB(255, 230, 230, 230);
            paintRadius.SetStyle(Paint.Style.Fill);
            // Shape for left displacement 
            m_ShapeRadiusLeft = new ShapeDrawable(new OvalShape());
            m_ShapeRadiusLeft.Paint.Set(paintRadius);
            // Shape for right displacement
            m_ShapeRadiusRight = new ShapeDrawable(new OvalShape());
            m_ShapeRadiusRight.Paint.Set(paintRadius);
        }

        /// <summary>
        /// Sets the bounds for every joystick and displacement oval
        /// </summary>
        private void InitJoysticks()
        {
            m_LeftJS = new Joystick(SCREEN_WIDTH, SCREEN_HEIGHT, true);
            m_RightJS = new Joystick(SCREEN_WIDTH, SCREEN_HEIGHT, false);

            m_ShapeStickLeft.SetBounds(
                (int)m_LeftJS.CENTER_X - (int)m_LeftJS.m_StickRadius, 
                (int)m_LeftJS.CENTER_Y + (int)m_LeftJS.m_StickRadius, 
                (int)m_LeftJS.CENTER_X + (int)m_LeftJS.m_StickRadius, 
                (int)m_LeftJS.CENTER_Y + 3*(int)m_LeftJS.m_StickRadius);

            m_ShapeStickRight.SetBounds(
                (int)m_RightJS.CENTER_X - (int)m_RightJS.m_StickRadius, 
                (int)m_RightJS.CENTER_Y - (int)m_RightJS.m_StickRadius,
                (int)m_RightJS.CENTER_X + (int)m_RightJS.m_StickRadius, 
                (int)m_RightJS.CENTER_Y + (int)m_RightJS.m_StickRadius);

            //m_ShapeRadiusLeft.SetBounds(
            //    (int)m_LeftJS.CENTER_X - (int)m_LeftJS.m_DisplacementRadius - (int)m_LeftJS.m_StickRadius, 
            //    (int)m_LeftJS.CENTER_Y - (int)m_LeftJS.m_DisplacementRadius - (int)m_LeftJS.m_StickRadius,
            //    (int)m_LeftJS.CENTER_X + (int)m_LeftJS.m_DisplacementRadius + (int)m_LeftJS.m_StickRadius, 
            //    (int)m_LeftJS.CENTER_Y + (int)m_LeftJS.m_DisplacementRadius + (int)m_LeftJS.m_StickRadius);

            m_ShapeRadiusLeft.SetBounds(
                (int)m_LeftJS.CENTER_X - (int)m_LeftJS.m_DisplacementRadius,
                (int)m_LeftJS.CENTER_Y - (int)m_LeftJS.m_DisplacementRadius,
                (int)m_LeftJS.CENTER_X + (int)m_LeftJS.m_DisplacementRadius,
                (int)m_LeftJS.CENTER_Y + (int)m_LeftJS.m_DisplacementRadius);

            m_ShapeRadiusRight.SetBounds(
                (int)m_RightJS.CENTER_X - (int)m_LeftJS.m_DisplacementRadius, 
                (int)m_RightJS.CENTER_Y - (int)m_RightJS.m_DisplacementRadius,
                (int)m_RightJS.CENTER_X + (int)m_RightJS.m_DisplacementRadius, 
                (int)m_RightJS.CENTER_Y + (int)m_RightJS.m_DisplacementRadius);
        }

        /// <summary>
        /// Checks single or multitouch and sets new bounds
        /// </summary>
        public bool OnTouch(View v, MotionEvent e)
        {

            switch(e.Action)
            {
                case MotionEventActions.Up:
                    //if(e.PointerCount == 2)
                    //{
                    //    if(e.GetX(0) > SCREEN_WIDTH / 2)
                    //    {
                    //        UpdateOvals(m_RightJS.CENTER_X, m_RightJS.CENTER_Y);
                    //    }
                    //    if(e.GetX(1) > SCREEN_WIDTH / 2)
                    //    {
                    //        UpdateOvals(m_RightJS.CENTER_X, m_RightJS.CENTER_Y);
                    //    }
                    //}
                    //if(e.GetX() > SCREEN_WIDTH / 2)
                    //{
                    //    UpdateOvals(m_RightJS.CENTER_X, m_RightJS.CENTER_Y);
                    //}
                    //else
                    //{
                    //    if(e.GetX() > SCREEN_WIDTH / 2)
                    //    {
                    //        UpdateOvals(m_RightJS.CENTER_X, m_RightJS.CENTER_Y);
                    //    }
                    //}
                    UpdateOvals(m_RightJS.CENTER_X, m_RightJS.CENTER_Y);
                    break;
                case MotionEventActions.Pointer1Up:
                    UpdateOvals(m_RightJS.CENTER_X, m_RightJS.CENTER_Y);
                    break;
                case MotionEventActions.Pointer2Up:
                    UpdateOvals(m_RightJS.CENTER_X, m_RightJS.CENTER_Y);
                    break;
                default:
                    UpdateOvals(e.GetX(0), e.GetY(0));
                    if (e.PointerCount == 2)
                    {
                        UpdateOvals(e.GetX(1), e.GetY(1));
                    }
                    break;
            }
            
            this.Invalidate();
            return true;
        }

        /// <summary>
        /// Sets new bounds for the joystick oval
        /// </summary>
        /// <param name="xPosition">X-Position of the touch</param>
        /// <param name="yPosition">Y-Position of the touch</param>
        private void UpdateOvals(float xPosition, float yPosition)
        {
            // Check if touch is in left or right of the screen
            if (xPosition <= SCREEN_WIDTH / 2)
            {
                // Handle touch in the left half
                m_LeftJS.SetPosition(xPosition, yPosition);
                m_LeftJS.GetPower();
                // Check if touch was inside the displacement radius
                if ((m_LeftJS.GetAbs()) <= m_LeftJS.m_DisplacementRadius) // if((abs + m_LeftJS.m_StickRadius...
                {
                    // Draw left joystick with original coordinates
                    SetBoundsForLeftStick(
                    (int)xPosition - (int)m_LeftJS.m_StickRadius,
                    (int)yPosition - (int)m_LeftJS.m_StickRadius,
                    (int)xPosition + (int)m_LeftJS.m_StickRadius,
                    (int)yPosition + (int)m_LeftJS.m_StickRadius);
                }
                else
                {
                    // Draw left joystick with maximum coordinates
                    SetBoundsForLeftStick(
                    (int)(m_LeftJS.m_DisplacementRadius * Math.Cos(m_LeftJS.GetAngle() * Math.PI / 180)) - (int)m_LeftJS.m_StickRadius + (int)m_LeftJS.CENTER_X,
                    (int)(m_LeftJS.m_DisplacementRadius * Math.Sin(m_LeftJS.GetAngle() * Math.PI / 180)) - (int)m_LeftJS.m_StickRadius + (int)m_LeftJS.CENTER_Y,
                    (int)(m_LeftJS.m_DisplacementRadius * Math.Cos(m_LeftJS.GetAngle() * Math.PI / 180)) + (int)m_LeftJS.m_StickRadius + (int)m_LeftJS.CENTER_X,
                    (int)(m_LeftJS.m_DisplacementRadius * Math.Sin(m_LeftJS.GetAngle() * Math.PI / 180)) + (int)m_LeftJS.m_StickRadius + (int)m_LeftJS.CENTER_Y);
                }
            }
            else
            {
                // Handle touch in the right half
                m_RightJS.SetPosition(xPosition, yPosition);
                // Check if touch was inside the displacement radius
                if ((m_RightJS.GetAbs()) <= m_RightJS.m_DisplacementRadius)
                {
                    // Draw right joystick with original coordinates
                   SetBoundsForRightStick(
                    (int)xPosition - (int)m_RightJS.m_StickRadius,
                    (int)yPosition - (int)m_RightJS.m_StickRadius,
                    (int)xPosition + (int)m_RightJS.m_StickRadius,
                    (int)yPosition + (int)m_RightJS.m_StickRadius);
                }
                else
                {
                    // Draw left joystick with maximum coordinates
                    SetBoundsForRightStick(
                    (int)(m_RightJS.m_DisplacementRadius * Math.Cos(m_RightJS.GetAngle() * Math.PI / 180)) - (int)m_RightJS.m_StickRadius + (int)m_RightJS.CENTER_X,
                    (int)(m_RightJS.m_DisplacementRadius * Math.Sin(m_RightJS.GetAngle() * Math.PI / 180)) - (int)m_RightJS.m_StickRadius + (int)m_RightJS.CENTER_Y,
                    (int)(m_RightJS.m_DisplacementRadius * Math.Cos(m_RightJS.GetAngle() * Math.PI / 180)) + (int)m_RightJS.m_StickRadius + (int)m_RightJS.CENTER_X,
                    (int)(m_RightJS.m_DisplacementRadius * Math.Sin(m_RightJS.GetAngle() * Math.PI / 180)) + (int)m_RightJS.m_StickRadius + (int)m_RightJS.CENTER_Y);
                }
            }
        }

        /// <summary>
        /// Draws the shapes onto the canvas, which is displayed afterwards
        /// </summary>
        protected override void OnDraw(Canvas canvas)
        {
            // Draw shapes
            m_ShapeRadiusLeft.Draw(canvas);
            m_ShapeRadiusRight.Draw(canvas);
            m_ShapeStickLeft.Draw(canvas);
            m_ShapeStickRight.Draw(canvas);
            

            // Set paint for data text
            Paint paint = new Paint();
            paint.SetARGB(255, 0, 0, 0);
            paint.TextSize = 20;
            paint.TextAlign = Paint.Align.Center;
            paint.StrokeWidth = 5;

            // Draw data text for left joystick
            canvas.DrawText("DATA LEFT JOYSTICK", m_LeftJS.CENTER_X, m_LeftJS.CENTER_Y - SCREEN_HEIGHT / 2 - 30, paint);
            canvas.DrawText("Throttle " + m_LeftJS.GetThrottleValue(), m_LeftJS.CENTER_X, m_LeftJS.CENTER_Y - SCREEN_HEIGHT / 2, paint);
            canvas.DrawText("Rotation " + m_LeftJS.GetRotationValue(), m_LeftJS.CENTER_X, m_LeftJS.CENTER_Y - SCREEN_HEIGHT / 2 + 30, paint);
            canvas.DrawText("Angle is " + m_LeftJS.GetAngle() + " °", m_LeftJS.CENTER_X, m_LeftJS.CENTER_Y - SCREEN_HEIGHT / 2 + 60, paint);
            canvas.DrawText("Direction is " + m_LeftJS.GetDirection(), m_LeftJS.CENTER_X, m_LeftJS.CENTER_Y - SCREEN_HEIGHT / 2 + 90, paint);

            // Draw data text for right joystick
            canvas.DrawText("DATA RIGHT JOYSTICK", m_RightJS.CENTER_X, m_RightJS.CENTER_Y - SCREEN_HEIGHT / 2 - 30, paint);
            canvas.DrawText("Power is " + m_RightJS.GetPower() + " %", m_RightJS.CENTER_X, m_RightJS.CENTER_Y - SCREEN_HEIGHT / 2, paint);
            canvas.DrawText("Abs is " + m_RightJS.GetAbs(), m_RightJS.CENTER_X, m_RightJS.CENTER_Y - SCREEN_HEIGHT / 2 + 30, paint);
            canvas.DrawText("Angle is " + m_RightJS.GetAngle() + " °", m_RightJS.CENTER_X, m_RightJS.CENTER_Y - SCREEN_HEIGHT / 2 + 60, paint);
            canvas.DrawText("Direction is " + m_RightJS.GetDirection(), m_RightJS.CENTER_X, m_RightJS.CENTER_Y - SCREEN_HEIGHT / 2 + 90, paint);


        }

        /// <summary>
        /// Helper method for setting the bounds of the left joystick
        /// </summary>
        /// <param name="left">Position of left bound</param>
        /// <param name="top">Position of top bound</param>
        /// <param name="right">Position of right bound</param>
        /// <param name="bottom">Position of bottom bound</param>
        private void SetBoundsForLeftStick(int left, int top, int right, int bottom)
        {
            m_ShapeStickLeft.SetBounds(left, top, right, bottom);
        }

        /// <summary>
        /// Helper method for setting the bounds of the right joystick
        /// </summary>
        /// <param name="left">Position of left bound</param>
        /// <param name="top">Position of top bound</param>
        /// <param name="right">Position of right bound</param>
        /// <param name="bottom">Position of bottom bound</param>
        private void SetBoundsForRightStick(int left, int top, int right, int bottom)
        {
            m_ShapeStickRight.SetBounds(left, top, right, bottom);
        }

        /// <summary>
        /// Helper method for converting pixels (=px) into
        /// density-independent pixels (=dp)
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns>Converted value in dp</returns>
        private int ConvertPixelsToDp(float pixels)
        {
            return (int)((pixels) / Resources.DisplayMetrics.Density);
        }
    }
}