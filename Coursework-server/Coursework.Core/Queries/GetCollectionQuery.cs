using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetCollectionQuery : IRequest<CollectionVm>
{
    public Guid Id { get; }

    public GetCollectionQuery(Guid id)
    {
        Id = id;
    }
}