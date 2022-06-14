using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetShortItemsQuery : IRequest<List<ShortItemVm>>
{
    public Guid CollectionId { get; }

    public GetShortItemsQuery(Guid collectionId)
    {
        CollectionId = collectionId;
    }
}