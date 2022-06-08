using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetShortItemsQuery : IRequest<List<ShortItemVm>>
{
    public Guid CollectionId { get; }

    public GetShortItemsQuery(Guid collectionId)
    {
        CollectionId = collectionId;
    }
}