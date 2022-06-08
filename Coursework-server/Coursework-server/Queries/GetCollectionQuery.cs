using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetCollectionQuery : IRequest<CollectionVm>
{
    public Guid Id { get; }

    public GetCollectionQuery(Guid id)
    {
        Id = id;
    }
}