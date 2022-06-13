using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

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