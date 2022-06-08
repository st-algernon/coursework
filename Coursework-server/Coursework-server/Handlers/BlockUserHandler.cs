using Coursework_server.Data;
using Coursework_server.Data.Models;
using Coursework_server.Handlers.Base;
using Coursework_server.Queries;
using MediatR;

namespace Coursework_server.Handlers;

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