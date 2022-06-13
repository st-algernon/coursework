using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

internal class GetCollectionHandler : BaseCollectionHandler, IRequestHandler<GetCollectionQuery, CollectionVm>
{
	public GetCollectionHandler(AppDbContext db) : base(db)
	{ }

	public async Task<CollectionVm> Handle(GetCollectionQuery request, CancellationToken cancellationToken) =>
		ConvertHelper.ToCollectionVm(await GetCollectionByIdAsync(request.Id, cancellationToken));
}