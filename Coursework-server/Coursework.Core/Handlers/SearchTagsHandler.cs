using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

public class SearchTagsHandler : IRequestHandler<SearchTagsQuery, List<TagVm>>
{
    private readonly AppDbContext _db;

    public SearchTagsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TagVm>> Handle(SearchTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _db.Tags
            .Where(t => t.Name.Contains(request.Name))
            .ToListAsync(cancellationToken);
        var tagVMs = tags.Select(ConvertHelper.ToTagVm).ToList();

        return tagVMs;
    }
}