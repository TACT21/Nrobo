namespace RobotManager.Input
{
    public abstract class SensorInterface:IDisposable
    {
        public virtual void Dispose() { }
        public virtual byte[] Read() {
            throw new NotImplementedException();
        }
    }
}
