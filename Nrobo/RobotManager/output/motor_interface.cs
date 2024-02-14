using System.Device.Pwm.Drivers;
using System.Device.Gpio;

namespace RobotManager.output
{
    public abstract class MotorInterface : IDisposable
    {
        protected SoftwarePwmChannel channel;
        protected MotorPinGuide guide;
        protected GpioController controller;

        public MotorInterface(MotorPinGuide guide, int Hz)
        {
            this.guide = guide;
            channel = new SoftwarePwmChannel(guide.PwmChannel, Hz);
            controller = new GpioController(guide.Scheme);
            controller.OpenPin(guide.In1, PinMode.Output);
            controller.OpenPin(guide.In2, PinMode.Output);
        }

        public virtual void Dispose()
        {
            channel.Dispose();
            controller.ClosePin(guide.In1);
            controller.ClosePin(guide.In2);
            controller.Dispose();
        }

        public virtual void Move(double speed) { }
    }

    public class MotorPinGuide
    {
        public sbyte In1 { private set; get; } = -1;
        public sbyte In2 { private set; get; } = -1;
        public sbyte PwmChannel { private set; get; } = -1;
        public PinNumberingScheme Scheme { private set; get; } = PinNumberingScheme.Logical;

        public MotorPinGuide(sbyte In1, sbyte In2, PinNumberingScheme Scheme = 0)
        {
            this.In1 = In1;
            this.In2 = In2;
            this.Scheme = Scheme;
        }

        public MotorPinGuide(sbyte In1,
            sbyte In2,
            sbyte PwmChannel,
            PinNumberingScheme Scheme = PinNumberingScheme.Logical)
        {
            this.In1 = In1;
            this.In2 = In2;
            this.Scheme = Scheme;
            this.PwmChannel = PwmChannel;
        }
    }
}
