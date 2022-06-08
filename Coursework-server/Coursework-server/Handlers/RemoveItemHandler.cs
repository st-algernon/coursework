using Coursework_server.Commands;
using Coursework_server.Data;
using Coursework_server.Handlers.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class RemoveItemHandler : BaseItemHandler, IRequestHandler<RemoveItemCommand>
{
    public RemoveItemHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var item = await GetItemByIdAsync(request.Id, cancellationToken);
        
        Db.Items.Remove(item);
        await Db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}