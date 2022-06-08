using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

internal class GetShortCollectionHandler : BaseCollectionHandler, IRequestHandler<GetShortCollectionQuery, ShortCollectionVm>
{
	public GetShortCollectionHandler(AppDbContext db) : base(db)
	{
	}

	public async Task<ShortCollectionVm> Handle(GetShortCollectionQuery request, CancellationToken cancellationToken) =>
		ConvertHelper.ToShortCollectionVm(await GetCollectionByIdAsync(request.Id, cancellationToken));
}