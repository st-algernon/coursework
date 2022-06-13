using Coursework.Core.Data;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class GetUsersCountHandler : IRequestHandler<GetUsersCountQuery, int>
{
    private readonly AppDbContext _db;

    public GetUsersCountHandler(AppDbContext db)
    {
        _db = db;
    }

    public Task<int> Handle(GetUsersCountQuery request, CancellationToken cancellationToken)
    {
        return _db.Users.CountAsync(cancellationToken);
    }
}