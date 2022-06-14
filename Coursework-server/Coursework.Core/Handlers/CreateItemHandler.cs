using Coursework.Core.Commands;
using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Handlers.Base;
using MediatR;

namespace Coursework.Core.Handlers;

internal class CreateItemHandler : BaseItemHandler, IRequestHandler<CreateItemCommand>
{
    public CreateItemHandler(AppDbContext db) : base (db)
    { }

    public async Task<Unit> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        await CheckRightsToModifyItemsAsync(request.CollectionId, request.CurrentUserId, cancellationToken);

        var item = new Item
        {
            Title = request.Title,
            CoverUrl = request.CoverUrl,
            CollectionId = request.CollectionId,
            CreationDate = DateTime.UtcNow,
        };

        Db.Items.Add(item);

        item.Tags = await GetTagsOrCreateAsync(request.TagNames, cancellationToken);
        item.ItemFields = request.FullFieldVMs.Select(f => new FieldItem
        {
            FieldId = f.Id,
            Value = f.Value
        }).ToList();

        await Db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}