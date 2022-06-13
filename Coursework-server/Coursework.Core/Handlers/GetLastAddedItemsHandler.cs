using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class GetLastAddedItemsHandler : IRequestHandler<GetLastAddedItemsQuery, List<ShortItemVm>>
{
    private readonly AppDbContext _db;

    public GetLastAddedItemsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ShortItemVm>> Handle(GetLastAddedItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _db.Items
            .OrderByDescending(i => i.CreationDate)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Include(i => i.ItemFields)
            .ThenInclude(i => i.Field)
            .ThenInclude(i => i.FieldType)
            .Include(i => i.Tags)
            .ToListAsync(cancellationToken);
        var itemVMs = items
            .Select(ConvertHelper.ToShortItemVm)
            .ToList();

        return itemVMs;
    }
}