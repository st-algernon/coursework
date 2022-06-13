using MediatR;

namespace Coursework.Core.Queries;

public class BlockUserQuery : IRequest<Unit>
{
    public Guid Id { get; }

    public BlockUserQuery(Guid id)
    {
        Id = id;
    }
}