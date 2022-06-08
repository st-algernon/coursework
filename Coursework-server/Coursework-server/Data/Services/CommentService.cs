using Coursework_server.Commands;
using Coursework_server.Data.Models;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Data.Services
{
    public class CommentService
    {
        private readonly AppDbContext _db;

        public CommentService(AppDbContext context)
        {
            _db = context;
        }

        public async Task<Comment> AddCommentAsync(CreateCommentCommand request)
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

        public async Task<Comment> LoadAuhtorAsync(Comment comment)
        {
            await _db.Entry(comment).Reference(m => m.Author).LoadAsync();

            return comment;
        }

        public List<Comment> GetCommentsAsync(Guid itemId) => _db.Comments
            .Where(c => c.ItemId == itemId)
            .OrderByDescending(c => c.CreationDate)
            .Include(c => c.Author)
            .ToList();

        public List<CommentVm> GetCommentVMsAsync(Guid itemId)
        {
            var comments = GetCommentsAsync(itemId);
            var commentVMs = comments.Select(ConvertHelper.ToCommentVm).ToList();

            return commentVMs;
        }
    }
}
