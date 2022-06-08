using Coursework_server.Commands;
using Coursework_server.Data;
using Coursework_server.Data.Models;
using Coursework_server.Handlers.Base;
using MediatR;

namespace Coursework_server.Handlers;

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