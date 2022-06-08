using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetShortCollectionQuery : IRequest<ShortCollectionVm>
{
    public Guid Id { get; }

    public GetShortCollectionQuery(Guid id)
    {
        Id = id;
    }
}