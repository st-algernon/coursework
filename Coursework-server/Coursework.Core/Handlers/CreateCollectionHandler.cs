using Coursework.Core.Commands;
using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Handlers.Base;
using MediatR;

namespace Coursework.Core.Handlers;

internal class CreateCollectionHandler : BaseCollectionHandler, IRequestHandler<CreateCollectionCommand>
{
    public CreateCollectionHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
    {
        await CheckRightsToModifyCollectionsAsync(request.OwnerId, request.CurrentUserId, cancellationToken);
        
        Db.Collections.Add(new Collection
        {
            CoverUrl = request.CoverUrl ?? string.Empty,
            Title = request.Title,
            Description = request.Description,
            TopicId = request.TopicId,
            OwnerId = request.OwnerId,
            Fields = request.FieldVMs
                .Select(f => new Field
                {
                    Name = f.Name,
                    FieldTypeId = f.FieldTypeId
                }).ToList()
        });

        await Db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}