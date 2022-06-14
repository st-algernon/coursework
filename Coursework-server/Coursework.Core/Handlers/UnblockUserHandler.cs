using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

internal class UnblockUserHandler : BaseUserHandler, IRequestHandler<UnblockUserQuery>
{
    public UnblockUserHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(UnblockUserQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserByIdAsync(request.Id, cancellationToken);

        user.UserState = UserState.Active;

        await Db.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}