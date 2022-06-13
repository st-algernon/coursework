using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class GetCollectionFieldsHandler : BaseCollectionHandler, IRequestHandler<GetCollectionFieldsQuery, List<FieldWithTypeNameVm>>
{
    public GetCollectionFieldsHandler(AppDbContext db) : base(db)
    { }

    public async Task<List<FieldWithTypeNameVm>> Handle(GetCollectionFieldsQuery request, CancellationToken cancellationToken)
    {
        await CheckIfCollectionExistAsync(cancellationToken);

        var fields = await Db.Fields
            .Where(c => c.CollectionId == request.Id)
            .Include(f => f.FieldType)
            .ToListAsync(cancellationToken: cancellationToken);
        var fieldWithTypeNameVMs = fields.Select(ConvertHelper.ToFieldWithTypeNameVm).ToList();

        return fieldWithTypeNameVMs;
    }
}