using Coursework.Core.Data;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

internal class RemoveUserHandler : BaseUserHandler, IRequestHandler<RemoveUserQuery>
{
    public RemoveUserHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(RemoveUserQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserByIdAsync(request.Id, cancellationToken);
        
        Db.Users.Remove(user);
        await Db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}