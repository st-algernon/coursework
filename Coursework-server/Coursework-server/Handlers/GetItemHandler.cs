using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class GetItemHandler : BaseItemHandler, IRequestHandler<GetItemQuery, ItemVm>
{
    public GetItemHandler(AppDbContext db) : base(db)
    { }

    public async Task<ItemVm> Handle(GetItemQuery request, CancellationToken cancellationToken) =>
        ConvertHelper.ToItemVm(await GetItemByIdAsync(request.Id, cancellationToken), request.CurrentUserId);
}