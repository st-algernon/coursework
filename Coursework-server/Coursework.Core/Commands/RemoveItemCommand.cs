using MediatR;

namespace Coursework.Core.Commands;

public class RemoveItemCommand : IRequest<Unit>
{
    public Guid Id { get; }

    public RemoveItemCommand(Guid id)
    {
        Id = id;
    }
}