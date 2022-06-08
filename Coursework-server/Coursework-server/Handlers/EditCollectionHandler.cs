using Coursework_server.Commands;
using Coursework_server.Data;
using Coursework_server.Data.Models;
using Coursework_server.Data.ViewModels;
using Coursework_server.Extensions;
using Coursework_server.Handlers.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

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