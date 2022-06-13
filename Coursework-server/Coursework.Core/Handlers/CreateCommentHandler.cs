using Coursework.Core.Commands;
using Coursework.Core.Data;
using Coursework.Core.Data.Models;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Helpers;
using MediatR;

namespace Coursework.Core.Handlers;

internal class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentVm>
{
    private readonly AppDbContext _db;

    public CreateCommentHandler(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<CommentVm> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        if (request.AuthorId.ToString() != request.CurrentUserId)
        {
            throw new InvalidOperationException();
        }
        
        var comment = await AddCommentAsync(request);
        var commentWithAuthor = await LoadAuthorAsync(comment);
        var commentVm = ConvertHelper.ToCommentVm(commentWithAuthor);

        return commentVm;
    }
    
    private async Task<Comment> AddCommentAsync(CreateCommentCommand request)
    {
        var comment = new Comment
        {
            Text = request.Text,
            AuthorId = request.AuthorId,
            CreationDate = DateTime.UtcNow,
            ItemId = request.ItemId
        };

        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();

        return comment;
    }

    private async Task<Comment> LoadAuthorAsync(Comment comment)
    {
        await _db.Entry(comment).Reference(m => m.Author).LoadAsync();

        return comment;
    }
}