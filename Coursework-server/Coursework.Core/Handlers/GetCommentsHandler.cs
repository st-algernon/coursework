using Coursework.Core.Data;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class GetCommentsHandler : IRequestHandler<GetCommentsQuery, List<CommentVm>>
{
    private readonly AppDbContext _db;

    public GetCommentsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CommentVm>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _db.Comments
            .Where(c => c.ItemId == request.ItemId)
            .OrderByDescending(c => c.CreationDate)
            .Include(c => c.Author)
            .ToListAsync(cancellationToken: cancellationToken);
        var commentVMs = comments.Select(ConvertHelper.ToCommentVm).ToList();

        return commentVMs;
    }
}