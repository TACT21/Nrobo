namespace RobotManager.Shared
{
    public class InputInfo
    {
        public UnitsNet.Temperature Temperature { get; set; }
        public UnitsNet.Pressure Pressure { get; set; }
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
        public DetaContainer(Type type,object obj) {
            this.Type = type;
            this.Obj = System.Text.Json.JsonSerializer.Serialize(obj,type);
        }
        public DetaContainer(dynamic obj)
        {
            this.Type = obj.GetType();
            this.Obj = System.Text.Json.JsonSerializer.Serialize(obj,this.Type);
        }
    }
}