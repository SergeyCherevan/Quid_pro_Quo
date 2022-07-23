using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Quid_pro_Quo.SignalRHubs
{
    public class MessagingHub : Hub
    {
        public async Task SendToServer(string message)
        {
            await this.Clients.All.SendAsync("SendToClient", $"MessagingHub response: you write \"{message}\"");
        }
    }
}
