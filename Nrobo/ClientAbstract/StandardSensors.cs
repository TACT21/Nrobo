using System;
using System.Collections.Generic;
using System.Text;
using UnitsNet.Units;
using UnitsNet;
using RobotManager.Shared;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace RobotManager.StandardSensors
{
    public class SpeedMeter : Sensor
    {
        private readonly Type Unit = typeof(Speed);
        private Func<string, Task<DetaContainer>> Getter;
        private string CallKey = "GetSpeed";
        private string _Value = "";
        private string Value
        {
            set
            {
                _Value = value;
                var candidate = (DetaContainer?)JsonSerializer.Deserialize(Value, typeof(DetaContainer));
                if (candidate != null && candidate.Type == Unit)
                {
                    ValueDeSerialized = (Speed)JsonSerializer.Deserialize(candidate.Obj, Unit);
                }
            }

            get { return _Value; }
        }
        private Speed ValueDeSerialized { set; get; }
        public SpeedMeter(Func<string, Task<DetaContainer>> getter):base(getter)
        {
            Getter = getter;
        }
    }

    public class DistanceWall : Sensor
    {
        private readonly Type Unit = typeof(Length);
        private Func<string, Task<DetaContainer>> Getter;
        private string CallKey = "GetSpeed";
        private string _Value = "";
        private string Value
        {
            set
            {
                _Value = value;
                var candidate = (DetaContainer?)JsonSerializer.Deserialize(Value, typeof(DetaContainer));
                if (candidate != null && candidate.Type == Unit)
                {
                    ValueDeSerialized = (Length)JsonSerializer.Deserialize(candidate.Obj, Unit);
                }
            }

            get { return _Value; }
        }
        private Length ValueDeSerialized { set; get; }
        public DistanceWall(Func<string, Task<DetaContainer>> getter) : base(getter)
        {
            Getter = getter;
        }
    }

    public class Barometer : Sensor
    {
        private readonly Type Unit = typeof((Temperature,Pressure));
        private Func<string, Task<DetaContainer>> Getter;
        private string CallKey = "GetTemp";
        private string _Value = "";
        private string Value
        {
            set
            {
                _Value = value;
                var candidate = (DetaContainer?)JsonSerializer.Deserialize(Value, typeof(DetaContainer));
                if (candidate != null && candidate.Type == Unit)
                {
                    ValueDeSerialized = ((Temperature,Pressure))JsonSerializer.Deserialize(candidate.Obj, Unit);
                }
            }

            get { return _Value; }
        }
        private (Temperature,Pressure) ValueDeSerialized { set; get; }
        public SpeedMeter(Func<string, Task<DetaContainer>> getter) : base(getter)
        {
            Getter = getter;
        }
    }

    public abstract class Atm : Sensor
    {
        private readonly Type Unit = typeof(Ratio);
        private Func<string, Task<DetaContainer>> Getter;
        private string CallKey = "GetSpeed";
        private string _Value = "";
        private string Value
        {
            set
            {
                _Value = value;
                var candidate = (DetaContainer?)JsonSerializer.Deserialize(Value, typeof(DetaContainer));
                if (candidate != null && candidate.Type == Unit)
                {
                    ValueDeSerialized = (Ratio)JsonSerializer.Deserialize(candidate.Obj, Unit);
                }
            }

            get { return _Value; }
        }
        private Ratio ValueDeSerialized { set; get; }
        public DistanceWall(Func<string, Task<DetaContainer>> getter) : base(getter)
        {
            Getter = getter;
        }
    }

    public class CORatio : Atm
    {
        private string CallKey = "GetCO";
    }

    public class GasRatio : Atm
    {
        private string CallKey = "GetGas";
    }

    public class MethaneRatio : Atm
    {
        private string CallKey = "GetMethane";
    }

    public class AlcoholRatio : Atm
    {
        private string CallKey = "GetAlcohol";
    }
}
