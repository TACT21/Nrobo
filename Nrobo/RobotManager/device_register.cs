using Iot.Device.Bmp180;
using RobotManager.Input;
using RobotManager.output;
using System.Device.Gpio;

namespace RobotManager
{
    public static class DeviceRegister
    {
        private static MotorInterface? rightMotor = null;
        public static MotorInterface? RightMotor
        {
            get { return rightMotor; }
            set
            {
                if (rightMotor != null)
                {
                    throw new IndexOutOfRangeException("This value is only set once.");
                }
                rightMotor = value;
            }
        }

        private static MotorInterface? leftMotor = null;
        public static MotorInterface? LeftMotor
        {
            get { return leftMotor; }
            set
            {
                if (leftMotor != null)
                {
                    throw new IndexOutOfRangeException("This value is only set once.");
                }
                leftMotor = value;
            }
        }

        private static Bmp180? bmp180 = null;
        public static Bmp180? Bmp180 {
            get { return bmp180; }
            set
            {
                if (bmp180 != null)
                {
                    throw new IndexOutOfRangeException("This value is only set once.");
                }
                bmp180 = value;
            }
        }

        public static Dictionary<string, SensorInterface> Sensors { set; get; } = new();

        private static Led? light;
        public static Led? Light
        {
            get { return light; }
            set
            {
                if (light != null)
                {
                    throw new IndexOutOfRangeException("This value is only set once.");
                }
                light = value;
            }
        }

        public static readonly PinNumberingScheme pinNumberingScheme = PinNumberingScheme.Logical;
    }
}
