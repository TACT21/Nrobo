using UnitsNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RobotManager.Shared
{
    public class InputInfo
    {
        public Temperature Temperature { get; set; }
        public Pressure Pressure { get; set; }
    }

    public class Enums
    {
        public enum Direction
        {
            Left = 0,
            Right = 1
        }
    }

    public class DetaContainer
    {
        public Type Type { get; set; }
        public string Obj { get; set; }
        public DetaContainer(Type type, object obj)
        {
            this.Type = type;
            this.Obj = System.Text.Json.JsonSerializer.Serialize(obj, type);
        }
        public DetaContainer(dynamic obj)
        {
            this.Type = obj.GetType();
            this.Obj = System.Text.Json.JsonSerializer.Serialize(obj, this.Type);
        }
    }
}