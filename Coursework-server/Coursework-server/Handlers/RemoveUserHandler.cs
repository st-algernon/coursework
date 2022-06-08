using Coursework_server.Data;
using Coursework_server.Handlers.Base;
using Coursework_server.Queries;
using MediatR;

namespace Coursework_server.Handlers;

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