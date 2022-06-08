using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetCollectionTagsQuery : IRequest<List<TagVm>>
{
    public Guid Id { get; }

    public GetCollectionTagsQuery(Guid id)
    {
        Id = id;
    }
}