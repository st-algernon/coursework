using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Commands;

public class CreateCommentCommand : IRequest<CommentVm>
{
    public string Text { get; set; }
    public Guid AuthorId { get; set; }
    public Guid ItemId { get; set; }
    public string? CurrentUserId { get; set; }
}