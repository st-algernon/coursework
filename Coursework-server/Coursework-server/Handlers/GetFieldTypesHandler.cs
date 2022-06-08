using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

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