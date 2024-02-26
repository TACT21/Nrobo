using System.Device.Gpio;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using UnitsNet;
using System.Collections.Generic;
using RobotManager.Shared;
using System;

namespace RobotManager.Input
{
    /// <summary>
    /// This is the management class for HC-SR04
    /// For tehcnical reason, this class does not operate in Windows OS.
    /// </summary>
    public class HCSR04: HardwareInterface
    {
        public readonly int TrigPin;
        public readonly int EchoPin;
        private readonly GpioController controller;
        private const int SpeedOfSound = 332; //μm/μs

        public HCSR04 (int trigPin, int echoPin,PinNumberingScheme scheme){
            // check the system timer accuracy whether within 100μ second
            if((1000L*1000L) / Stopwatch.Frequency > 100){
                throw  new PlatformNotSupportedException
                    ("HCSR04 class is not use in the machine which can not provide system timer accuracy within 10μ second.");
            };
            TrigPin = trigPin;
            EchoPin = echoPin;
            controller = new GpioController (scheme);
            controller.OpenPin (trigPin,PinMode.Output,false);
            controller.OpenPin (echoPin,PinMode.Input);
        }
        public void Dispose() {
            controller.ClosePin(TrigPin);
            controller.ClosePin(EchoPin);
            controller.Dispose();
        }
        
        public DetaContainer Read() {
            controller.Write(TrigPin,PinValue.High);
            Task.Delay(TimeSpan.FromMicroseconds(10));
            controller.Write(TrigPin,PinValue.Low);

            var st = DateTime.Now;
            DateTime? end = null;
            
            while (true){
                if(controller.Read(EchoPin) == PinValue.High){
                    end = DateTime.Now;
                    break;
                }
            }


            if(end != null){
                TimeSpan span = ((DateTime)end).Subtract(st);
                var result = UnitsNet.Length.FromMicrometers(span.TotalMicroseconds * 278);
                return new DetaContainer(typeof(UnitsNet.Length), result);
            }else
            {
                return new DetaContainer(null , null);
            }
        }

        public async Task<DetaContainer> ReadAsync()
        {
            controller.Write(TrigPin, PinValue.High);
            Task.Delay(TimeSpan.FromMicroseconds(10));
            controller.Write(TrigPin, PinValue.Low);


            var st = DateTime.Now;
            DateTime? end = null;

            while (true)
            {
                if (controller.Read(EchoPin) == PinValue.High)
                {
                    end = DateTime.Now;
                    break;
                }
            }

            var calc = Task.Run(()=>
            {
                if (end != null)
                {
                    TimeSpan span = ((DateTime)end).Subtract(st);
                    var result = UnitsNet.Length.FromMicrometers(span.TotalMicroseconds * 278);
                    return new DetaContainer(typeof(UnitsNet.Length), result);
                }
                else
                {
                    return new DetaContainer(null, null);
                }
            });

            return await calc;
        }
    }
}
