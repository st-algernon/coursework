using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

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