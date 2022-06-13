using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers.Base;

internal abstract class BaseItemHandler
{
    protected readonly AppDbContext Db;

    protected BaseItemHandler(AppDbContext db)
    {
        Db = db;
    }

    protected async Task<Item> GetItemByIdAsync(Guid id, CancellationToken ct)
    {
        var item = await Db.Items
            .Include(i => i.ItemFields)
            .ThenInclude(i => i.Field)
            .ThenInclude(i => i.FieldType)
            .Include(i => i.ItemUsers)
            .Include(i => i.Tags)
            .Include(i => i.Collection)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

        if (item == null)
        {
            throw new InvalidOperationException();
        }

        return item;
    }
    
    protected async Task<List<Tag>> GetTagsOrCreateAsync(List<string> tagNames, CancellationToken ct)
    {
        var tags = new List<Tag>();

        foreach (var name in tagNames)
        {
            var tag = await Db.Tags.FirstOrDefaultAsync(t => t.Name == name, ct);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = name
                };

                Db.Tags.Add(tag);
            }

            tags.Add(tag);
        }

        return tags;
    }

    protected async Task CheckRightsToModifyItemsAsync(Guid collectionId, string? currentUserId, CancellationToken ct)
    {
        if (currentUserId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var currentUser = await Db.Users
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(currentUserId), ct);
        var collection = await Db.Collections
            .FirstOrDefaultAsync(c => c.Id == collectionId, ct);

        if (collection?.OwnerId != currentUser?.Id || currentUser?.UserRole != UserRole.Admin)
        {
            throw new InvalidOperationException();
        }
    }
}