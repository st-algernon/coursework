using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetItemQuery : IRequest<ItemVm>
{
    public Guid Id { get; }
    public string? CurrentUserId { get; }
    
    public GetItemQuery(Guid id, string? currentUserId)
    {
        Id = id;
        CurrentUserId = currentUserId;
    }
}