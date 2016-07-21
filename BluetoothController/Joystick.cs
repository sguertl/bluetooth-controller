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

namespace Controller
{
    public class Joystick
    {
        // -------------------------- CONSTANTS --------------------------------

        private const double RAD = 1 / (2 * Math.PI) * 360; // 1 rad in degrees

        public static readonly int CENTER = 0;
        public static readonly int BOTTOM = 1;
        public static readonly int BOTTOM_RIGHT = 2;
        public static readonly int RIGHT = 3;
        public static readonly int TOP_RIGHT = 4;
        public static readonly int TOP = 5;
        public static readonly int TOP_LEFT = 6;
        public static readonly int LEFT = 7;
        public static readonly int BOTTOM_LEFT = 8;
        public static readonly int LEFT_STICK = 0;
        public static readonly int RIGHT_STICK = 1;

        public readonly float StickDiameter; // Diameter of the joystick
        public readonly float DisplacementDiameter; // Diameter of the displacement

        public static float StickRadius; // Radius of the joystick
        public static float DisplacementRadius; // Radius of the displacement


        // --------------------------- VARIABLES ------------------------------

        private float m_XPosition; // Current x of joystick
        private float m_YPosition; // Current y of joystick

        // Center x of joystick
        private float m_CenterX; 
        public float CenterX { get { return m_CenterX; } set { m_CenterX = value; } }

        // Center y of joystick
        private float m_CenterY; 
        public float CenterY { get { return m_CenterY; } set { m_CenterY = value; } }

        // Current power (= displacement) of the joystick; maximum is 1 (= 100 %)
        private int m_Power; 
        public int Power { get { return GetPower(); } set { m_Power = value; } }

        // Current angle of the joystick
        private float m_Angle; 
        public float Angle { get { return GetAngle(); } set { m_Angle = value; } }

        // Current direction of the joystick
        private int m_Direction;
        public int Direction { get { return GetDirection(); } set { m_Direction = value; } }

        // Vector length
        private float m_Abs;
        public float Abs { get { return GetAbs(); } set { m_Abs = value; } }


        private Int16 m_ThrottleValue;
        public Int16 ThrottleValue { get { return GetThrottleValue(); } set { m_ThrottleValue = value; } }

        private Int16 m_RotationValue;
        public Int16 RotationValue { get { return GetRotationValue(); } set { m_RotationValue = value; } }

        private Int16 m_ForwardBackwardValue;
        public Int16 ForwardBackwardValue { get { return GetForwardBackwardValue(); } set { m_ForwardBackwardValue = value; } }

        private Int16 m_LeftRightValue;
        public Int16 LeftRightValue { get { return GetLeftRightValue(); } set { m_LeftRightValue = value; } }


        // ----------------------------- CTOR ----------------------------------
        public Joystick(float width, float height, bool isLeftStick, bool invertedControl)
        {
            StickDiameter = (width / 8 + width / 2) / 2 - width / 5;
            DisplacementDiameter = StickDiameter * 2.25f;

            StickRadius = StickDiameter / 2;
            DisplacementRadius = DisplacementDiameter / 2;

            m_CenterY = height / 16 + height / 2 + StickRadius / 2;
            if(!invertedControl)
            {
                if (isLeftStick)
                {
                    m_CenterX = width / 5 + StickRadius / 2;
                    SetPosition(m_CenterX, m_CenterY + DisplacementRadius);
                }
                else
                {
                    m_CenterX = width - width / 5 - StickRadius / 2;
                    SetPosition(m_CenterX, m_CenterY);
                }
            }
            else
            {
                if (isLeftStick)
                {
                    m_CenterX = width / 5 + StickRadius / 2;
                    SetPosition(m_CenterX, m_CenterY);
                }
                else
                {
                    m_CenterX = width - width / 5 - StickRadius / 2;
                    SetPosition(m_CenterX, m_CenterY + DisplacementRadius);                   
                }
            }
        }

        /// <summary>
        /// Sets the current position of the joystick
        /// </summary>
        /// <param name="xPosition">X-Position of the joystick</param>
        /// <param name="yPosition">Y-Position of the joystick</param>
        public void SetPosition(float xPosition, float yPosition)
        {
            m_XPosition = xPosition;
            m_YPosition = yPosition;
        }

        /// <summary>
        /// Returns the current position of the joystick
        /// </summary>
        /// <returns>An array containing the x and y position of the joystick</returns>
        public float[] GetPosition()
        {
            return new float[] { m_XPosition, m_YPosition };
        }

        /// <summary>
        /// Calculates the angle of the moved joystick
        /// </summary>
        /// <returns>Angle of the joystick</returns>
        private float GetAngle()
        {
            if (m_XPosition > m_CenterX)
            {
                if (m_YPosition < m_CenterY)
                {
                    //return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD + 90);
                    return m_Angle = (int)(Math.Atan((m_YPosition - m_CenterY) / (m_XPosition - m_CenterX)) * RAD + 90) - 90;
                }
                else if (m_YPosition > m_CenterY)
                {
                    //return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD) + 90;
                    return m_Angle = (int)(Math.Atan((m_YPosition - m_CenterY) / (m_XPosition - m_CenterX)) * RAD);
                }
                else
                {
                    //return m_Angle = 90;
                    return m_Angle = 0;
                }
            }
            else if (m_XPosition < m_CenterX)
            {
                if (m_YPosition < m_CenterY)
                {
                    //return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD - 90);
                    return m_Angle = (int)(Math.Atan((m_YPosition - m_CenterY) / (m_XPosition - m_CenterX)) * RAD - 90) - 90;
                }
                else if (m_YPosition > m_CenterY)
                {
                    //return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD) - 90;
                    return m_Angle = (int)(Math.Atan((m_YPosition - m_CenterY) / (m_XPosition - m_CenterX)) * RAD) - 180;
                }
                else
                {
                    //return m_Angle = -90;
                    return m_Angle = -180;
                }
            }
            else
            {
                if (m_YPosition <= m_CenterY)
                {
                    //return m_Angle = 0;
                    return m_Angle = -90;
                }
                else
                {
                    if (m_Angle < 0)
                    {
                        //return m_Angle = -180;
                        return m_Angle = -270;
                    }
                    else
                    {
                        //return m_Angle = 180;
                        return m_Angle = 90;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the direction in which the joystick was moved
        /// </summary>
        /// <returns>Direction of the joystick</returns>
        private int GetDirection()
        {
            if(m_CenterX == m_XPosition && m_CenterY == m_YPosition)
            {
                return m_Direction = 0;
            }
            if (m_Power == 0 && m_Angle == 0)
            {
                return m_Direction = 0;
            }
            int a = 0;
            if (m_Angle <= 0)
            {
                a = ((int)m_Angle * -1) + 90;
            }
            else if (m_Angle > 0)
            {
                if (m_Angle <= 90)
                {
                    a = 90 - (int)m_Angle;
                }
                else
                {
                    a = 360 - ((int)m_Angle - 90);
                }
            }

            m_Direction = (int)(((a + 22) / 45) + 1);

            if (m_Direction > 8)
            {
                m_Direction = 1;
            }
            return m_Direction;
        }

        /// <summary>
        /// Calculates the power of the joystick
        /// </summary>
        /// <returns>Power of the joystick in percent (max. 100)</returns>
        private int GetPower()
        {
            m_Power = (int)(100 * Math.Sqrt(
                (m_XPosition - m_CenterX) * (m_XPosition - m_CenterX) + 
                (m_YPosition - m_CenterY) * (m_YPosition - m_CenterY)) / (DisplacementRadius));
            m_Power = Math.Min(m_Power, 100);
            return m_Power;
        }

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        /// <returns>Length of the vector</returns>
        private float GetAbs()
        {
            return m_Abs = (float)Math.Sqrt((m_XPosition - m_CenterX) * (m_XPosition - m_CenterX) + (m_YPosition - m_CenterY) * (m_YPosition - m_CenterY));
        }

        private Int16 GetThrottleValue()
        {
            int throttleValue = 0;
            if (m_YPosition > m_CenterY + DisplacementRadius)
            {
                return (Int16)throttleValue;
            }
            throttleValue = (int)(32767 * (m_CenterY + DisplacementRadius - m_YPosition) / DisplacementDiameter);
            throttleValue = Math.Max((Int16)0, throttleValue);
            throttleValue = Math.Min((Int16)32767, throttleValue);
            m_ThrottleValue = (Int16)throttleValue;
            return m_ThrottleValue;
        }

        private Int16 GetRotationValue()
        {
            int rotationValue = -32768;
            if (m_XPosition < m_CenterX - DisplacementRadius)
            {
                return (Int16)rotationValue;
            }
            rotationValue = (int)((65536 * (m_CenterX + DisplacementRadius - m_XPosition) / DisplacementDiameter) - 32768) * (-1);
            rotationValue = Math.Max(-32768, rotationValue);
            rotationValue = Math.Min(32767, rotationValue);
            m_RotationValue = (Int16)rotationValue;
            return m_RotationValue;
        }

        private Int16 GetForwardBackwardValue()
        {
            int forwardBackwardValue = -32768;
            if (m_YPosition > m_CenterY + DisplacementRadius)
            {
                return (Int16)forwardBackwardValue;
            }
            forwardBackwardValue = (int)(65536 * (m_CenterY + DisplacementRadius - m_YPosition) / DisplacementDiameter) - 32768;
            forwardBackwardValue = Math.Max(-32768, forwardBackwardValue);
            forwardBackwardValue = Math.Min(32767, forwardBackwardValue);
            m_ForwardBackwardValue = (Int16)forwardBackwardValue;
            return m_ForwardBackwardValue;
        }

        private Int16 GetLeftRightValue()
        {
            int leftRightValue = -32768;
            if (m_XPosition < m_CenterX - DisplacementRadius)
            {
                return (Int16)leftRightValue;
            }
            leftRightValue = (int)((65536 * (m_CenterX + DisplacementRadius - m_XPosition) / DisplacementDiameter) - 32768) * (-1);
            leftRightValue = Math.Max(-32768, leftRightValue);
            leftRightValue = Math.Min(32767, leftRightValue);
            m_LeftRightValue = (Int16)leftRightValue;
            return m_LeftRightValue;
        }

        public void CalculateValues()
        {
            GetPower();
            GetAngle();
            GetAbs();
        }
    }
}