using Coursework_server.Data;
using Coursework_server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers.Base;

internal abstract class BaseCollectionHandler
{
	// ReSharper disable once MemberCanBePrivate.Global
	protected readonly AppDbContext Db;

	protected BaseCollectionHandler(AppDbContext db)
	{
		Db = db;
	}

	protected async Task<Collection> GetCollectionByIdAsync(Guid id, CancellationToken ct)
	{
		var collection = await Db.Collections
			.Include(c => c.Topic)
			.Include(c => c.Fields)
			.FirstOrDefaultAsync(c => c.Id == id, ct);

		if (collection == null)
		{
			throw new InvalidOperationException();
		}

		return collection;
	}
    
    protected async Task CheckRightsToModifyCollectionsAsync(Guid ownerId, string? currentUserId, CancellationToken ct)
    {
        if (currentUserId == null)
        {
            throw new UnauthorizedAccessException();
        }
                        
        var currentUser = await Db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(currentUserId), ct);
        
        if (ownerId != currentUser?.Id || currentUser.UserRole != UserRole.Admin)
        {
            throw new InvalidOperationException();
        }
    }

    protected async Task CheckIfCollectionExistAsync(CancellationToken ct)
    {
        var result = await Db.Collections.AnyAsync(ct);

        if (result == false)
        {
            throw new InvalidOperationException();
        }
    }
}