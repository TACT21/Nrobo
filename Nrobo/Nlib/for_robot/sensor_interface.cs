using RobotManager.Shared;
using System;
using System.Threading.Tasks;

namespace RobotManager.Input
{
    public interface HardwareInterface : IDisposable
    {
        public void Dispose() { }
        public DetaContainer Read()
        {
            throw new NotImplementedException();
        }
        public async Task<DetaContainer> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
