using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class GetUserCollectionsHandler : IRequestHandler<GetUserCollectionsQuery, List<ShortCollectionVm>>
{
    private readonly AppDbContext _db;

    public GetUserCollectionsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ShortCollectionVm>> Handle(GetUserCollectionsQuery request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.AnyAsync(c => c.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == false)
        {
            throw new InvalidOperationException();
        }

        var collections = await _db.Collections
            .Where(c => c.OwnerId == request.UserId)
            .Include(c => c.Topic)
            .ToListAsync(cancellationToken);
        var shortCollectionVMs = collections.Select(ConvertHelper.ToShortCollectionVm).ToList();

        return shortCollectionVMs;
    }
}