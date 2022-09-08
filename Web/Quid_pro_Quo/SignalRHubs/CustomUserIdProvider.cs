using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Quid_pro_Quo.SignalRHubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
