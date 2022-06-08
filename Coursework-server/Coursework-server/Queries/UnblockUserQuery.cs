using MediatR;

namespace Coursework_server.Queries;

public class UnblockUserQuery : IRequest<Unit>
{
    public Guid Id { get; }

    public UnblockUserQuery(Guid id)
    {
        Id = id;
    }
}