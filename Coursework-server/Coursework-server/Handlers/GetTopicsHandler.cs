using Coursework_server.Data;
using Coursework_server.Data.Models;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class GetTopicsHandler : IRequestHandler<GetTopicsQuery, List<Topic>>
{
    private readonly AppDbContext _db;

    public GetTopicsHandler(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Topic>> Handle(GetTopicsQuery request, CancellationToken cancellationToken)
    {
        return _db.Topics.ToListAsync(cancellationToken);
    }
}