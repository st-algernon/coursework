using Coursework_server.Commands;
using Coursework_server.Data;
using Coursework_server.Data.Hubs;
using Coursework_server.Data.Models;
using Coursework_server.Helpers;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Coursework_server.Handlers;

internal class CreateCommentHandler : IRequestHandler<CreateCommentCommand>
{
    private readonly IHubContext<CommentsHub> _hubContext;
    private readonly AppDbContext _db;

    public CreateCommentHandler(IHubContext<CommentsHub> hubContext, AppDbContext db)
    {
        _hubContext = hubContext;
        _db = db;
    }
    
    public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        if (request.AuthorId.ToString() != request.CurrentUserId)
        {
            throw new InvalidOperationException();
        }
        
        var comment = await AddCommentAsync(request);
        var commentWithAuthor = await LoadAuthorAsync(comment);
        var commentVm = ConvertHelper.ToCommentVm(commentWithAuthor);

        await _hubContext.Clients.All.SendAsync("Receive", commentVm, cancellationToken);

        return Unit.Value;
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