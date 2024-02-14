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
}