using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System.Net;
using UnitsNet;
using UnitsNet.Units;
using System.Text.Json;
using RobotManager.Shared;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ClientAbstract
{
    public interface SensorInterface
    {
        public dynamic? Read() { throw new NotImplementedException(); }
        public string? ReadSerialized (){ throw new NotImplementedException(); }
        public async Task<dynamic?> ReadAsnyc() { throw new NotImplementedException(); }
        public async Task<string> ReadAsnycSerialized() { throw new NotImplementedException(); }
    }
    /// <summary>
    /// Abstract Class of Sensor dealing
    /// </summary>
    public abstract class Sensor : SensorInterface
    {
        private const Type Unit = null;
        private Func<string,Task<DetaContainer>> Getter;
        private string CallKey="";
        private string _Value = "";
        private string Value
        {
            set
            {
                _Value = value;
                var candidate = (DetaContainer?)JsonSerializer.Deserialize(Value, typeof(DetaContainer));
                if (candidate != null && candidate.Type == Unit) {
                    ValueDeSerialized = JsonSerializer.Deserialize(candidate.Obj, Unit);
                }
            }

            get { return _Value; }
        }
        private object? ValueDeSerialized { set; get; }
        /// <summary>
        /// Initializer of this class.
        /// </summary>
        /// <param name="getter">
        /// Communicate agent with robot.
        /// </param>
        /// <exception cref="NotImplementedException">Throw when any oversight is there.</exception>
        public Sensor(Func<string,Task<DetaContainer>> getter) {
            if(Unit == null & CallKey != "")
            {
                throw new NotImplementedException();
            }
            Getter = getter;
        }
        /// <summary>
        /// Reading Value
        /// </summary>
        public dynamic? Read(){ return ValueDeSerialized; }
        /// <summary>
        /// Reading Raw Value
        /// </summary>
        public string ReadSerialized() { return Value; }
        /// <summary>
        /// Take and Reading Value from Robot
        /// </summary>
        public async Task<dynamic?> ReadAsync() {
            await Retrieve();
            return Read();
        }
        /// <summary>
        /// Take and Reading Raw Value from Robot
        /// </summary>
        public async Task<string> ReadAsyncSerialized()
        {
            await Retrieve();
            return ReadSerialized();
        }
        /// <summary>
        /// Take Raw Value from Robot
        /// </summary>
        public async Task Retrieve()
        {
            Value = await Getter(CallKey);
        }
    }
}