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
        // Constants
        private readonly double RAD = 1 / (2 * Math.PI) * 360; // 1 rad in degrees

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

        public readonly float m_StickDiameter; // Diameter of the joystick
        public readonly float m_DisplacementDiameter; // Diameter of the displacement

        public readonly float m_StickRadius; // Radius of the joystick
        public readonly float m_DisplacementRadius; // Radius of the displacement

        public readonly float CENTER_X; // Center x of joystick
        public readonly float CENTER_Y; // Center y of joystick

        // Variables
        private float m_XPosition; // Current x of joystick
        private float m_YPosition; // Current y of joystick

        private int m_StickIndex; // Index of joystick

        private int m_Power; // Current power (= displacement) of the joystick; maximum is 1 (= 100 %)

        private int m_Angle; // Current angle of the joystick

        private int m_Direction; // Current direction of the joystick

        public Joystick(float width, float height, bool isLeftStick, bool invertedControl)
        {
            m_StickDiameter = (width / 8 + width / 2) / 2 - width / 5;
            m_DisplacementDiameter = m_StickDiameter * 2.25f;

            m_StickRadius = m_StickDiameter / 2;
            m_DisplacementRadius = m_DisplacementDiameter / 2;

            CENTER_Y = height / 16 + height / 2 + m_StickRadius / 2;
            if(!invertedControl)
            {
                if (isLeftStick)
                {
                    CENTER_X = width / 5 + m_StickRadius / 2;
                    m_StickIndex = 0;
                    SetPosition(CENTER_X, CENTER_Y + m_DisplacementRadius);
                }
                else
                {
                    CENTER_X = width - width / 5 - m_StickRadius / 2;
                    m_StickIndex = 1;
                    SetPosition(CENTER_X, CENTER_Y);
                }
            }
            else
            {
                if (isLeftStick)
                {
                    CENTER_X = width / 5 + m_StickRadius / 2;
                    m_StickIndex = 0;
                    SetPosition(CENTER_X, CENTER_Y);
                }
                else
                {
                    CENTER_X = width - width / 5 - m_StickRadius / 2;
                    m_StickIndex = 1;
                    SetPosition(CENTER_X, CENTER_Y + m_DisplacementRadius);                   
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
        public float GetAngle()
        {
            if (m_XPosition > CENTER_X)
            {
                if (m_YPosition < CENTER_Y)
                {
                    //return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD + 90);
                    return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD + 90) - 90;
                }
                else if (m_YPosition > CENTER_Y)
                {
                    //return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD) + 90;
                    return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD);
                }
                else
                {
                    //return m_Angle = 90;
                    return m_Angle = 0;
                }
            }
            else if (m_XPosition < CENTER_X)
            {
                if (m_YPosition < CENTER_Y)
                {
                    //return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD - 90);
                    return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD - 90) - 90;
                }
                else if (m_YPosition > CENTER_Y)
                {
                    //return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD) - 90;
                    return m_Angle = (int)(Math.Atan((m_YPosition - CENTER_Y) / (m_XPosition - CENTER_X)) * RAD) - 180;
                }
                else
                {
                    //return m_Angle = -90;
                    return m_Angle = -180;
                }
            }
            else
            {
                if (m_YPosition <= CENTER_Y)
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
        public int GetDirection()
        {
            if(CENTER_X == m_XPosition && CENTER_Y == m_YPosition)
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
                a = (m_Angle * -1) + 90;
            }
            else if (m_Angle > 0)
            {
                if (m_Angle <= 90)
                {
                    a = 90 - m_Angle;
                }
                else
                {
                    a = 360 - (m_Angle - 90);
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
        public int GetPower()
        {
            m_Power = (int)(100 * Math.Sqrt(
                (m_XPosition - CENTER_X) * (m_XPosition - CENTER_X) + 
                (m_YPosition - CENTER_Y) * (m_YPosition - CENTER_Y)) / (m_DisplacementRadius));
            m_Power = Math.Min(m_Power, 100);
            return m_Power;
        }

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        /// <returns>Length of the vector</returns>
        public float GetAbs()
        {
            return (float)Math.Sqrt((m_XPosition - CENTER_X) * (m_XPosition - CENTER_X) + (m_YPosition - CENTER_Y) * (m_YPosition - CENTER_Y));
        }

        public int GetThrottleValue()
        {
            if (m_YPosition > CENTER_Y + m_DisplacementRadius)
            {
                return 0;
            }
            int throttleValue = (int)(32767 * Math.Sqrt(
            (m_XPosition - CENTER_X) * (m_XPosition - CENTER_X) +
            (m_YPosition - (CENTER_Y + m_DisplacementRadius)) * (m_YPosition - (CENTER_Y + m_DisplacementRadius))) / (m_DisplacementDiameter));
            throttleValue = Math.Max(0, throttleValue);
            throttleValue = Math.Min(32767, throttleValue);
            return throttleValue;
        }

        public int GetRotationValue()
        {
            if (m_XPosition < CENTER_X - m_DisplacementRadius)
            {
                return -32768;
            }
            int rotationValue = (int)(65536 * Math.Sqrt(
            (m_XPosition - (CENTER_X - m_DisplacementRadius)) * (m_XPosition - (CENTER_X - m_DisplacementRadius)) +
            (m_YPosition - CENTER_Y) * (m_YPosition - CENTER_Y)) / (m_DisplacementDiameter)) - 32768;
            rotationValue = Math.Max(-32768, rotationValue);
            rotationValue = Math.Min(32767, rotationValue);
            return rotationValue;
        }
    }
}