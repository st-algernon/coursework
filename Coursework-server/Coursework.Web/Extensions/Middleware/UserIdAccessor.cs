using Microsoft.AspNetCore.SignalR;

namespace Coursework.Web.Extensions.Middleware;

public class UserIdAccessor : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User.Identity?.Name;
    }
}