using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetCurrentUserQuery : IRequest<UserVm>
{
    public string? CurrentUserId { get; }

    public GetCurrentUserQuery(string? currentUserId)
    {
        CurrentUserId = currentUserId;
    }
}