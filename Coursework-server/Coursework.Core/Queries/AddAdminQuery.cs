using MediatR;

namespace Coursework.Core.Queries;

public class AddAdminQuery : IRequest<Unit>
{
    public Guid Id { get; }

    public AddAdminQuery(Guid id)
    {
        Id = id;
    }
}