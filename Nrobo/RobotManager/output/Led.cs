using System.Device.Gpio;

namespace RobotManager.output
{
    public class Led : IDisposable
    {
        private int ledPin = 0;
        private GpioController controller;
        private PinValue state = PinValue.Low;

        public Led(int pin, PinNumberingScheme scheme)
        {
            ledPin = pin;
            controller = new GpioController (scheme);
            controller.OpenPin (ledPin,PinMode.Output,state);
        }

        public void Dispose()
        {
            controller.Dispose();
        }

        public void Turn(PinValue value)
        {
            controller.Write(ledPin, value);
            state = value;
        }
        public void Turn()
        {
            state = (state == PinValue.High ? PinValue.Low : PinValue.High);
            controller.Write(ledPin, state);
        }
    }
}
