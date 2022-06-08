using Coursework_server.Commands;
using Coursework_server.Data;
using Coursework_server.Data.Models;
using Coursework_server.Handlers.Base;
using MediatR;

namespace Coursework_server.Handlers;

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