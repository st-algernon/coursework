using Coursework_server.Data;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

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