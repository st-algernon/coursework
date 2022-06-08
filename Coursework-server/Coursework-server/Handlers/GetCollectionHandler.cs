using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;

namespace Coursework_server.Handlers;

internal class GetCollectionHandler : BaseCollectionHandler, IRequestHandler<GetCollectionQuery, CollectionVm>
{
	public GetCollectionHandler(AppDbContext db) : base(db)
	{ }

	public async Task<CollectionVm> Handle(GetCollectionQuery request, CancellationToken cancellationToken) =>
		ConvertHelper.ToCollectionVm(await GetCollectionByIdAsync(request.Id, cancellationToken));
}