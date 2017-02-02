using System;

namespace Controller
{
    public class Joystick
    {
        // -------------------------- CONSTANTS --------------------------------

        private const double RAD = 1 / (2 * Math.PI) * 360; // 1 rad in degrees

        // Constant direction values
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
        private readonly bool m_LeftStick; // Side of stick
        private readonly bool m_Inverted; // Control mode

        // Center x of joystick
        private float m_CenterX; 
        public float CenterX { get { return m_CenterX; } private set { m_CenterX = value; } }

        // Center y of joystick
        private float m_CenterY; 
        public float CenterY { get { return m_CenterY; } private set { m_CenterY = value; } }

        // Current power (= displacement) of the joystick; maximum is 1 (= 100 %)
        private int m_Power; 
        public int Power { get { return GetPower(); } private set { m_Power = value; } }

        // Current angle of the joystick
        private float m_Angle; 
        public float Angle { get { return GetAngle(); } private set { m_Angle = value; } }

        // Current direction of the joystick
        private int m_Direction;
        public int Direction { get { return GetDirection(); } private set { m_Direction = value; } }

        // Vector length
        private float m_Abs;
        public float Abs { get { return GetAbs(); } private set { m_Abs = value; } }

        // Throttle value of the stick
        private Int16 m_Throttle;
        public Int16 Throttle { get { return GetThrottleValue(); } private set { m_Throttle = value; } }

        // Rudder value of the stick
        private Int16 m_Rudder;
        public Int16 Rudder { get { return GetRudderValue(); } private set { m_Rudder = value; } }

        // Elevator value of the stick
        private Int16 m_Elevator;
        public Int16 Elevator { get { return GetElevatorValue(); } private set { m_Elevator = value; } }

        // Aileron value of the stick
        private Int16 m_Aileron;
        public Int16 Aileron { get { return GetAileronValue(); } private set { m_Aileron = value; } }


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
            m_LeftStick = isLeftStick;
            m_Inverted = invertedControl;
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
            if((int)m_CenterX == (int)m_XPosition && (int)m_CenterY == (int)m_YPosition)
            {
                return m_Direction = 0;
            }
            if (m_Power == 0 && (int)m_Angle == 0)
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

            m_Direction = ((a + 22) / 45) + 1;

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

        /// <summary>
        /// Calculates the throttle value of the stick
        /// </summary>
        /// <returns>Throttle value (between 0 and 32767)</returns>
        private Int16 GetThrottleValue()
        {
            int throttleValue = 0;
            if (m_YPosition > m_CenterY + DisplacementRadius)
            {
                return (Int16)throttleValue;
            }
<<<<<<< Updated upstream
            throttleValue = (int)(40 * (m_CenterY + DisplacementRadius - m_YPosition) / DisplacementDiameter);
            throttleValue = Math.Max((Int16)0, throttleValue);
            throttleValue = Math.Min((Int16)40, throttleValue);
=======
            //throttleValue = (int)(32767 * (m_CenterY + DisplacementRadius - m_YPosition) / DisplacementDiameter);
            throttleValue = (int)(90 * (m_CenterY + DisplacementRadius - m_YPosition) / DisplacementDiameter);
            throttleValue = Math.Max((Int16)0, throttleValue);
            //throttleValue = Math.Min((Int16)32767, throttleValue);
            throttleValue = Math.Min((Int16)255, throttleValue);
>>>>>>> Stashed changes
            m_Throttle = (Int16)throttleValue;
            return m_Throttle;
        }

        /// <summary>
        /// Calculates the rudder value of the stick
        /// </summary>
        /// <returns>Rudder value (between -32768 and 32767)</returns>
        private Int16 GetRudderValue()
        {
            //int rudderValue = -32768;
            int rudderValue = -90;
            if (m_XPosition < m_CenterX - DisplacementRadius)
            {
                return (Int16)rudderValue;
            }
            //rudderValue = (int)((65536 * (m_CenterX + DisplacementRadius - m_XPosition) / DisplacementDiameter) - 32768) * (-1);
            rudderValue = (int)((180 * (m_CenterX + DisplacementRadius - m_XPosition) / DisplacementDiameter) - 90) * (-1);
            //rudderValue = Math.Max(-32768, rudderValue);
            rudderValue = Math.Max(-90, rudderValue);
            //rudderValue = Math.Min(32767, rudderValue);
            rudderValue = Math.Min(90, rudderValue);
            m_Rudder = (Int16)rudderValue;
            return m_Rudder;
        }

        /// <summary>
        /// Calculates the elevator value of the stick
        /// </summary>
        /// <returns>Elevator value (between -32768 and 32767)</returns>
        private Int16 GetElevatorValue()
        {
            //int elevatorValue = -32768;
            int elevatorValue = -100;
            if (m_YPosition > m_CenterY + DisplacementRadius)
            {
                return (Int16)elevatorValue;
            }
<<<<<<< Updated upstream
            elevatorValue = (int)(40 * (m_CenterY + DisplacementRadius - m_YPosition) / DisplacementDiameter) -20;
            elevatorValue = Math.Max(0, elevatorValue);
            elevatorValue = Math.Min(40, elevatorValue);
=======
            //elevatorValue = (int)(65536 * (m_CenterY + DisplacementRadius - m_YPosition) / DisplacementDiameter) - 32768;
            elevatorValue = (int)(200 * (m_CenterY + DisplacementRadius - m_YPosition) / DisplacementDiameter) - 100;
            //elevatorValue = Math.Max(-32768, elevatorValue);
            elevatorValue = Math.Max(-100, elevatorValue);
            //elevatorValue = Math.Min(32767, elevatorValue);
            elevatorValue = Math.Min(100, elevatorValue);
>>>>>>> Stashed changes
            m_Elevator = (Int16)elevatorValue;
            return m_Elevator;
        }

        /// <summary>
        /// Calculates the aileron value of the stick
        /// </summary>
        /// <returns>Aileron value (between -32768 and 32767)</returns>
        private Int16 GetAileronValue()
        {
            //int aileronValue = -32768;
            int aileronValue = -100;
            if (m_XPosition < m_CenterX - DisplacementRadius)
            {
                return (Int16)aileronValue;
            }
            //aileronValue = (int)((65536 * (m_CenterX + DisplacementRadius - m_XPosition) / DisplacementDiameter) - 32768) * (-1);
            aileronValue = (int)((200 * (m_CenterX + DisplacementRadius - m_XPosition) / DisplacementDiameter) - 100) * (-1);
            //aileronValue = Math.Max(-32768, aileronValue);
            aileronValue = Math.Max(-100, aileronValue);
            //aileronValue = Math.Min(32767, aileronValue);
            aileronValue = Math.Min(100, aileronValue);
            m_Aileron = (Int16)aileronValue;
            return m_Aileron;
        }

        /// <summary>
        /// Helper method which calls GetPower(), GetAngle() and GetAbs()
        /// </summary>
        public void CalculateValues()
        {
            GetPower();
            GetAngle();
            GetAbs();
        }

        /// <summary>
        /// Checks if stick is currently centered
        /// </summary>
        /// <returns>True if stick is centered, false if not</returns>
        public bool IsCentered()
        {
            if (m_Inverted)
            {
                if (m_LeftStick)
                {
                    return (int)m_XPosition == (int)m_CenterX && (int)m_YPosition == (int)m_CenterY;
                }
                else
                {
                    return (int)m_XPosition == (int)m_CenterX && (int)m_YPosition == (int)(m_CenterY + DisplacementRadius);
                }
            }
            else
            { 
                if (m_LeftStick)
                {
                    return (int)m_XPosition == (int)m_CenterX && (int)m_YPosition == (int)(m_CenterY + DisplacementRadius);
                }
                else
                {
                    return (int)m_XPosition == (int)m_CenterX && (int)m_YPosition == (int)m_CenterY;
                }
            }
        }
    }
}