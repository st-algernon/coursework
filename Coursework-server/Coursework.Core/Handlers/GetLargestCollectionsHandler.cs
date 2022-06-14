using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

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