using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;

namespace Coursework_server.Handlers;

internal class GetCurrentUserHandler : BaseUserHandler, IRequestHandler<GetCurrentUserQuery, UserVm>
{
    public GetCurrentUserHandler(AppDbContext db) : base(db)
    { }

    public async Task<UserVm> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserByIdAsync(Guid.Parse(request.CurrentUserId ?? string.Empty), cancellationToken);

        return ConvertHelper.ToUserVm(user);
    }
}