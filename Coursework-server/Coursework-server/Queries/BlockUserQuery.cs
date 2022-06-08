using MediatR;

namespace Coursework_server.Queries;

public class BlockUserQuery : IRequest<Unit>
{
    public Guid Id { get; }

    public BlockUserQuery(Guid id)
    {
        Id = id;
    }
}