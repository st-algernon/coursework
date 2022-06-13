using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetCollectionTagsQuery : IRequest<List<TagVm>>
{
    public Guid Id { get; }

    public GetCollectionTagsQuery(Guid id)
    {
        Id = id;
    }
}