using MediatR;

namespace Coursework_server.Queries;

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