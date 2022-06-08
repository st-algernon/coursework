using Microsoft.AspNetCore.SignalR;

namespace Coursework_server.Extensions.Middleware
{
    public class UserIdAccessor : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity?.Name;
        }
    }
}
