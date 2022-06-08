using Coursework_server.Commands;
using Coursework_server.Data;
using Coursework_server.Handlers.Base;
using MediatR;

namespace Coursework_server.Handlers;

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