using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;

namespace Coursework_server.Handlers;

internal class GetUserHandler : BaseUserHandler, IRequestHandler<GetUserQuery, UserVm>
{
    public GetUserHandler(AppDbContext db) : base(db)
    { }

    public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken) => 
        ConvertHelper.ToUserVm(await GetUserByIdAsync(request.Id, cancellationToken));
}