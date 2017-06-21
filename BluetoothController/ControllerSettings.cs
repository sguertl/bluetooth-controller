using System;
namespace BluetoothController
{
    public class ControllerSettings
    {

        public static readonly bool ACTIVE = true;
        public static readonly bool INACTIVE = false;

        public bool Inverted
        {
            get;
            set;
        }

        public int TrimYaw
        {
            get;
            set;
        }

        public int TrimPitch
        {
            get;
            set;
        }

        public int TrimRoll
        {
            get;
            set;
        }

        public bool HeightControlActivated
        {
            get;
            set;
        }


        public ControllerSettings()
        {

        }
    }
}
