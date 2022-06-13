using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

internal class BlockUserHandler : BaseUserHandler, IRequestHandler<BlockUserQuery>
{
    public BlockUserHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(BlockUserQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserByIdAsync(request.Id, cancellationToken);

        user.UserState = UserState.Blocked;

        await Db.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}