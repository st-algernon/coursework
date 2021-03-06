using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public enum SearchBy
{
    Title,
    Tag,
    Comment
}

public class SearchItemsQuery : IRequest<List<ShortItemVm>>
{
    public string Query { get; set; }
    public SearchBy SearchBy { get; set; }
}