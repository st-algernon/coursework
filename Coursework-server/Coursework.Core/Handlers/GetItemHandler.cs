using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

internal class GetItemHandler : BaseItemHandler, IRequestHandler<GetItemQuery, ItemVm>
{
    public GetItemHandler(AppDbContext db) : base(db)
    { }

    public async Task<ItemVm> Handle(GetItemQuery request, CancellationToken cancellationToken) =>
        ConvertHelper.ToItemVm(await GetItemByIdAsync(request.Id, cancellationToken), request.CurrentUserId);
}