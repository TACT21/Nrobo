using System.Device.Gpio;
using System.Device.Pwm.Drivers;
using RobotManager.output;

namespace RobotManager.Output.Motor
{
    public class Ta7291p : MotorInterface
    {
        public Ta7291p(MotorPinGuide guide) : base(guide , 225) {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Move(double speed) {
            if(speed < 0)
            {
                base.controller.Write(base.guide.In1,PinValue.Low);
                base.controller.Write(base.guide.In2, PinValue.High);
            }
            else if (speed > 0)
            {
                base.controller.Write(base.guide.In1, PinValue.High);
                base.controller.Write(base.guide.In2, PinValue.Low);
            }
            else
            {
                base.controller.Write(base.guide.In1, PinValue.High);
                base.controller.Write(base.guide.In2, PinValue.High);
            }
        }
    }
}
