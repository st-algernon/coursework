using Coursework.Core.Commands;
using Coursework.Core.Data;
using Coursework.Core.Handlers.Base;
using MediatR;

namespace Coursework.Core.Handlers;

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