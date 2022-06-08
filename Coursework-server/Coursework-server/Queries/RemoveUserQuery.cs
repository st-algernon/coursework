using MediatR;

namespace Coursework_server.Queries;

public class RemoveUserQuery : IRequest<Unit>
{
    public Guid Id { get; }

    public RemoveUserQuery(Guid id)
    {
        Id = id;
    }
}