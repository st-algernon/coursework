using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

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