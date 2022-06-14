using MediatR;

namespace Coursework.Core.Commands;

public class RemoveCollectionCommand : IRequest<Unit>
{
    public Guid Id { get; }

    public RemoveCollectionCommand(Guid id)
    {
        Id = id;
    }
}