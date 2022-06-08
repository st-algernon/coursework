using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

public class SearchUsersHandler : IRequestHandler<SearchUsersQuery, List<UserVm>>
{
    private readonly AppDbContext _db;

    public SearchUsersHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<UserVm>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _db.Users
            .Where(u => EF.Functions.Like(u.Name, $"%{request.Query}%"))
            .ToListAsync(cancellationToken);
        var userVMs = users.Select(ConvertHelper.ToUserVm).ToList();
        
        return userVMs;
    }
}