using Coursework.Core.Commands;
using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Extensions;
using Coursework.Core.Handlers.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

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