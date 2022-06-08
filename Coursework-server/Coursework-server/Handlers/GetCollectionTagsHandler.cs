using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class GetCollectionTagsHandler : BaseCollectionHandler, IRequestHandler<GetCollectionTagsQuery, List<TagVm>>
{
    public GetCollectionTagsHandler(AppDbContext db) : base(db)
    { }

    public async Task<List<TagVm>> Handle(GetCollectionTagsQuery request, CancellationToken cancellationToken)
    {
        await CheckIfCollectionExistAsync(cancellationToken);

        var tags = await Db.Items
            .Where(i => i.CollectionId == request.Id)
            .SelectMany(i => i.Tags)
            .Distinct()
            .ToListAsync(cancellationToken: cancellationToken);
        var tagVMs = tags.Select(ConvertHelper.ToTagVm).ToList();

        return tagVMs;
    }
}