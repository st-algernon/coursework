using Coursework_server.Data;
using Coursework_server.Data.Models;
using Coursework_server.Handlers.Base;
using Coursework_server.Queries;
using MediatR;

namespace Coursework_server.Handlers;

internal class AddAdminHandler : BaseUserHandler, IRequestHandler<AddAdminQuery>
{
    public AddAdminHandler(AppDbContext db) : base (db)
    { }

    public async Task<Unit> Handle(AddAdminQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserByIdAsync(request.Id, cancellationToken);

        user.UserRole = UserRole.Admin;

        await Db.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}