using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetShortCollectionQuery : IRequest<ShortCollectionVm>
{
    public Guid Id { get; }

    public GetShortCollectionQuery(Guid id)
    {
        Id = id;
    }
}