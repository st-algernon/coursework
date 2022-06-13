using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetUserQuery : IRequest<UserVm>
{
    public Guid Id { get; }

    public GetUserQuery(Guid id)
    {
        Id = id;
    }
}