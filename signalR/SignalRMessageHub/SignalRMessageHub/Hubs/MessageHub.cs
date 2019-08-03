using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRMessageHub.Hubs
{
    [HubName("MessageHub")]
    public class MessageHub : Hub
    {
        public async Task SendMessage(string sender, string message)
        {
            await Clients.All.SendAsync("SRMessage", sender, message);
        }

    }
}
