using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

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