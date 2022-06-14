using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers.Base;

internal abstract class BaseUserHandler
{
    protected readonly AppDbContext Db;

    protected BaseUserHandler(AppDbContext db)
    {
        Db = db;
    }
    
    protected async Task<User> GetUserByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await Db.Users
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        if (user == null)
        {
            throw new InvalidOperationException();
        }

        return user;
    }
}