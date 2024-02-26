using Iot.Device.Bmp180;
using Microsoft.AspNetCore.SignalR;
using RobotManager.output;
using RobotManager.Output.Motor;
using System.Device.I2c;
using RobotManager.Shared;
using System.Text.Json;
using System.Device.Gpio;
using RobotManager.Input;

namespace RobotManager
{
    
    public class ControlManager : Hub
    {
        private uint connectionCount = 0;
        private GpioController gpioController = new (DeviceRegister.pinNumberingScheme);
        private static bool isInitialized = false;

        internal static void Initializer()
        {
            if (isInitialized)
            {
                throw new NotSupportedException("Initializer is supposed to be called ONLY ONCE in same process.");
            }

            MotorPinGuide lPinGuide = new(5,6,1);
            MotorPinGuide rPinGuide = new(7,8,0);
            DeviceRegister.RightMotor = new Ta7291p(rPinGuide);
            DeviceRegister.LeftMotor = new Ta7291p(lPinGuide);

            I2cConnectionSettings bmp180Setting = new(1, Bmp180.DefaultI2cAddress);
            DeviceRegister.Bmp180 = new Bmp180(I2cDevice.Create(bmp180Setting));

            DeviceRegister.Light = new(4,PinNumberingScheme.Logical);

            DeviceRegister.Sensors.Add("HCSR04", new HCSR04(23,24, PinNumberingScheme.Logical));

            string dll_dir = System.AppDomain.CurrentDomain.BaseDirectory + @"\plugins";
            foreach (string dll in System.IO.Directory.GetFiles(dll_dir, "*.dll"))
            {
                //load Assemblys
                var asm = System.Reflection.Assembly.LoadFrom(dll);

                foreach (Type type in asm.GetTypes())
                {
                    try
                    {
                    //Check inheritance
                        if (!(type is HardwareInterface)) continue;

                        //Check weather interface
                        if (type.IsInterface == true) continue;

                        //Register Sensor
                        DeviceRegister.Sensors.Add(
                            System.IO.Path.GetFileNameWithoutExtension(dll),
                            (HardwareInterface)Activator.CreateInstance(type)
                            );
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            if(connectionCount != 0)
            {
                connectionCount--;
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ConsoleWrite", "Connected", Context.ConnectionId);
            await base.OnConnectedAsync();
            connectionCount++;
        }

        internal async Task GiveInputInfo()
        {
            if(connectionCount > 0)
            {
                var inputInfo = new InputInfo();
                inputInfo.Temperature = DeviceRegister.Bmp180.ReadTemperature();
                inputInfo.Pressure = DeviceRegister.Bmp180.ReadPressure();
                await Clients.All.SendAsync("InputInfo", JsonSerializer.Serialize(inputInfo, typeof(InputInfo)));
            }
            await Task.Delay(1000);
            var noWait = GiveInputInfo();
        }

        public async Task Moving(Enums.Direction direction,double speed)
        {
            if(direction == Enums.Direction.Left)
            {
                DeviceRegister.LeftMotor.Move(speed);
            }else if(direction == Enums.Direction.Right)
            {
                DeviceRegister.RightMotor.Move(speed);
            }
        }

        public async Task Lighting(bool turn)
        {
            DeviceRegister.Light.Turn(turn ? PinValue.High : PinValue.Low);
        }

        public async Task Lighting(PinValue pinValue)
        {
            DeviceRegister.Light.Turn(pinValue);
        }
        
        public async Task Lighting()
        {
            DeviceRegister.Light.Turn();
        }

        public async Task<DetaContainer> ReadSensor(string sensorName)
        {
            return await DeviceRegister.Sensors[sensorName].ReadAsync();
        }
    }
}
