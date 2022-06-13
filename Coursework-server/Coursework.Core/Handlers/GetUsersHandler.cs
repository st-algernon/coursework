using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UserVm>>
{
    private readonly AppDbContext _db;

    public GetUsersHandler(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<UserVm>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return _db.Users
            .OrderBy(u => u.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(u => ConvertHelper.ToUserVm(u))
            .ToListAsync(cancellationToken);
    }
}