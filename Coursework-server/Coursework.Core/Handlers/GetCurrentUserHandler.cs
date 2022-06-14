using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Core.Handlers;

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