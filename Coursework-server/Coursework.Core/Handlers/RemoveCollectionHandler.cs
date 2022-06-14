using Coursework.Core.Commands;
using Coursework.Core.Data;
using Coursework.Core.Handlers.Base;
using MediatR;

namespace Coursework.Core.Handlers;

internal class RemoveCollectionHandler : BaseCollectionHandler, IRequestHandler<RemoveCollectionCommand>
{
    public RemoveCollectionHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(RemoveCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await GetCollectionByIdAsync(request.Id, cancellationToken);
        
        Db.Collections.Remove(collection);
        await Db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}