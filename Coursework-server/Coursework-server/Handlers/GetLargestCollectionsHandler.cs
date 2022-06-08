using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class GetLargestCollectionsHandler : IRequestHandler<GetLargestCollectionsQuery, List<ShortCollectionVm>>
{
    private readonly AppDbContext _db;

    public GetLargestCollectionsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ShortCollectionVm>> Handle(GetLargestCollectionsQuery request, CancellationToken cancellationToken)
    {
        var collections = await _db.Collections
            .OrderByDescending(c => c.Items.Count)
            .Include(c => c.Topic)
            .ToListAsync(cancellationToken);
        var shortCollectionVMs = collections.Select(ConvertHelper.ToShortCollectionVm).ToList();

        return shortCollectionVMs;
    }
}