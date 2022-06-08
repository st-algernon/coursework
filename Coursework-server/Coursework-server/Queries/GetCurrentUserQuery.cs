using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetCurrentUserQuery : IRequest<UserVm>
{
    public string? CurrentUserId { get; }

    public GetCurrentUserQuery(string? currentUserId)
    {
        CurrentUserId = currentUserId;
    }
}