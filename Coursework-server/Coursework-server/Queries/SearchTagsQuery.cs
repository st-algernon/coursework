using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class SearchTagsQuery : IRequest<List<TagVm>>
{
    public string Name { get; }

    public SearchTagsQuery(string name)
    {
        Name = name;
    }
}