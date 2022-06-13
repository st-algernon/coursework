using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

internal class GetShortCollectionHandler : BaseCollectionHandler, IRequestHandler<GetShortCollectionQuery, ShortCollectionVm>
{
	public GetShortCollectionHandler(AppDbContext db) : base(db)
	{
	}

	public async Task<ShortCollectionVm> Handle(GetShortCollectionQuery request, CancellationToken cancellationToken) =>
		ConvertHelper.ToShortCollectionVm(await GetCollectionByIdAsync(request.Id, cancellationToken));
}