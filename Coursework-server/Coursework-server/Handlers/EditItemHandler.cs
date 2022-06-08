using Coursework_server.Commands;
using Coursework_server.Data;
using Coursework_server.Data.Models;
using Coursework_server.Extensions;
using Coursework_server.Handlers.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class EditItemHandler : BaseItemHandler, IRequestHandler<EditItemCommand>
{
    public EditItemHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(EditItemCommand request, CancellationToken cancellationToken)
    {
        await CheckRightsToModifyItemsAsync(request.CollectionId, request.CurrentUserId, cancellationToken);

        var item = await Db.Items
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (item == null)
        {
            throw new InvalidOperationException();
        }

        request.CopyPropertiesTo(item);

        item.ItemFields = request.FullFieldVMs.Select(f => new FieldItem
        {
            FieldId = f.Id,
            Value = f.Value
        }).ToList();

        item.Tags = await GetTagsOrCreateAsync(request.TagNames, cancellationToken);

        await Db.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}