using MediatR;

namespace Coursework.Core.Queries;

public class LikeItemQuery : IRequest<Unit>
{
    public Guid Id { get; }
    public string? CurrentUserId { get; }
    
    public LikeItemQuery(Guid id, string? currentUserId)
    {
        Id = id;
        CurrentUserId = currentUserId;
    }
}