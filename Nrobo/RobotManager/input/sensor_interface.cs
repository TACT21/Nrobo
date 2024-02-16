namespace RobotManager.Input
{
    public abstract class SensorInterface:IDisposable
    {
        public virtual void Dispose() { }
        public virtual byte[] Read() {
            throw new NotImplementedException();
        }
        public async virtual Task<byte[]> ReadAsync() {
            throw new NotImplementedException();
        }
    }
}
