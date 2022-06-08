using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetUserCollectionsQuery : IRequest<List<ShortCollectionVm>>
{
    public Guid UserId { get; }

    public GetUserCollectionsQuery(Guid userId)
    {
        UserId = userId;
    }
}