using MediatR;

namespace Coursework_server.Queries;

public class AddAdminQuery : IRequest<Unit>
{
    public Guid Id { get; }

    public AddAdminQuery(Guid id)
    {
        Id = id;
    }
}