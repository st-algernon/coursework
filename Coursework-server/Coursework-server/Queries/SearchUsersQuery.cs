using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class SearchUsersQuery : IRequest<List<UserVm>>
{
    public string Query { get; }

    public SearchUsersQuery(string query)
    {
        Query = query;
    }
}