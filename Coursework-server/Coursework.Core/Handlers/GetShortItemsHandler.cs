using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class GetShortItemsHandler : IRequestHandler<GetShortItemsQuery, List<ShortItemVm>>
{
    private readonly AppDbContext _db;

    public GetShortItemsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ShortItemVm>> Handle(GetShortItemsQuery request, CancellationToken cancellationToken)
    {
        var items =  await _db.Items
            .Where(i => i.CollectionId == request.CollectionId)
            .Include(i => i.ItemFields)
            .ThenInclude(i => i.Field)
            .ThenInclude(i => i.FieldType)
            .Include(i => i.ItemUsers)
            .Include(i => i.Tags)
            .ToListAsync(cancellationToken);
        var itemVMs = items.Select(ConvertHelper.ToShortItemVm).ToList();

        return itemVMs;
    }
}