using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class GetTopTagsHandler : IRequestHandler<GetTopTagsQuery, List<TagVm>>
{
    private readonly AppDbContext _db;

    public GetTopTagsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TagVm>> Handle(GetTopTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _db.Tags
            .OrderByDescending(t => t.Items.Count)
            .ToListAsync(cancellationToken);
        var tagVMs = tags.Select(ConvertHelper.ToTagVm).ToList();

        return tagVMs;
    }
}