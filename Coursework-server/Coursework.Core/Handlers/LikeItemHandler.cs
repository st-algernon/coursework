using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

internal class LikeItemHandler : BaseItemHandler, IRequestHandler<LikeItemQuery>
{
    public LikeItemHandler(AppDbContext db) : base(db)
    { }

    public async Task<Unit> Handle(LikeItemQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = request.CurrentUserId;
        
        if (currentUserId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var item = await GetItemByIdAsync(request.Id, cancellationToken);

        await ToggleLikeItem(item, currentUserId, cancellationToken);

        return Unit.Value;
    }

    private async Task ToggleLikeItem(Item item, string currentUserId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var itemUser = item.ItemUsers.FirstOrDefault(i => i.UserId == Guid.Parse(currentUserId));

        if (itemUser != null)
        {
            item.ItemUsers.Remove(itemUser);
            
            await Db.SaveChangesAsync(cancellationToken);

            return;
        }

        item.ItemUsers.Add(new UserItem
        {
            UserId = Guid.Parse(currentUserId),
            IsLiked = true
        });

        await Db.SaveChangesAsync(cancellationToken);
    }
}