using Coursework_server.Data;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Handlers;

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