using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

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