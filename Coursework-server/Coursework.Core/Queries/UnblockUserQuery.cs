using MediatR;

namespace Coursework.Core.Queries;

public class UnblockUserQuery : IRequest<Unit>
{
    public Guid Id { get; }

    public UnblockUserQuery(Guid id)
    {
        Id = id;
    }
}