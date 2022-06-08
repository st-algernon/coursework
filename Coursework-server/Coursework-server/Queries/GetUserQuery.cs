using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetUserQuery : IRequest<UserVm>
{
    public Guid Id { get; }

    public GetUserQuery(Guid id)
    {
        Id = id;
    }
}