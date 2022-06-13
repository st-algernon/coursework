using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

internal class GetUserHandler : BaseUserHandler, IRequestHandler<GetUserQuery, UserVm>
{
    public GetUserHandler(AppDbContext db) : base(db)
    { }

    public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken) => 
        ConvertHelper.ToUserVm(await GetUserByIdAsync(request.Id, cancellationToken));
}