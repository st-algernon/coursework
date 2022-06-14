using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

public class SearchItemsHandler : IRequestHandler<SearchItemsQuery, List<ShortItemVm>>
{
    private readonly AppDbContext _db;

    public SearchItemsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ShortItemVm>> Handle(SearchItemsQuery request, CancellationToken cancellationToken)
    {
        var itemVMs = new List<ShortItemVm>();

        if (request.SearchBy == SearchBy.Tag)
        {
            var items = await _db.Tags
                .Where(t => t.Name == request.Query)
                .SelectMany(t => t.Items)
                .Include(i => i.ItemFields)
                .ThenInclude(i => i.Field)
                .ThenInclude(i => i.FieldType)
                .Include(i => i.ItemUsers)
                .Include(i => i.Tags)
                .ToListAsync(cancellationToken);
            itemVMs = items.Select(ConvertHelper.ToShortItemVm).ToList();
        }

        return itemVMs;
    }
}