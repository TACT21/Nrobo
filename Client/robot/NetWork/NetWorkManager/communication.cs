using Microsoft.AspNetCore.SignalR;

namespace NetWorkManager
{
    public class Communication : Hub
    {
        public async Task Identify(string user, string message)
        {
            await Clients.Caller.SendAsync("ServerIdentifyResult",  "Nrobo");
        }
    }
}
