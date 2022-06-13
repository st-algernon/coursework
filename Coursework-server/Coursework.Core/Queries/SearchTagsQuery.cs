using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class SearchTagsQuery : IRequest<List<TagVm>>
{
    public string Name { get; }

    public SearchTagsQuery(string name)
    {
        Name = name;
    }
}