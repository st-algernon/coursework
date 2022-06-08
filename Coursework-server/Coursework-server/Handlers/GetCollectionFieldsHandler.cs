using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

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