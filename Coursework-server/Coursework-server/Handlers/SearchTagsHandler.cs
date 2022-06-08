using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

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