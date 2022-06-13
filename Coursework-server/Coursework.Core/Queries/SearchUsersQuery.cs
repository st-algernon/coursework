using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class SearchUsersQuery : IRequest<List<UserVm>>
{
    public string Query { get; }

    public SearchUsersQuery(string query)
    {
        Query = query;
    }
}