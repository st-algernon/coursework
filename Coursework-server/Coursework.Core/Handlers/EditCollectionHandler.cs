using Coursework.Core.Commands;
using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Extensions;
using Coursework.Core.Handlers.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class EditCollectionHandler : BaseCollectionHandler, IRequestHandler<EditCollectionCommand>
{
    public EditCollectionHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(EditCollectionCommand request, CancellationToken cancellationToken)
    {
        await CheckRightsToModifyCollectionsAsync(request.OwnerId, request.CurrentUserId, cancellationToken);

        var collection = await Db.Collections
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (collection == null)
        {
            throw new InvalidOperationException();
        }
        
        request.CopyPropertiesTo(collection);

        collection.Fields = GetFieldsOrCreate(request.FieldVMs);

        await Db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
    
    private List<Field> GetFieldsOrCreate(List<FieldVm> fieldVMs)
    {
        var fields = new List<Field>();

        foreach (var fieldVm in fieldVMs)
        {
            var field = Db.Fields.FirstOrDefault(t => t.Name == fieldVm.Name) ?? new Field
            {
                Name = fieldVm.Name,
                FieldTypeId = fieldVm.FieldTypeId,
            };

            fields.Add(field);
        }

        return fields;
    }
}