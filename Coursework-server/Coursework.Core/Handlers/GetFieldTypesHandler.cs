using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class GetFieldTypesHandler : IRequestHandler<GetFieldTypesQuery, List<FieldTypeVm>>
{
    private readonly AppDbContext _db;

    public GetFieldTypesHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<FieldTypeVm>> Handle(GetFieldTypesQuery request, CancellationToken cancellationToken)
    {
        var fieldTypes = await _db.FieldTypes.ToListAsync(cancellationToken);
        var fieldTypeVMs = fieldTypes.Select(ConvertHelper.ToFieldTypeVm).ToList();

        return fieldTypeVMs;
    }
}