using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

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