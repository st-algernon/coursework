using MediatR;

namespace Coursework_server.Commands;

public class CreateCommentCommand : IRequest<Unit>
{
    public string Text { get; set; }
    public Guid AuthorId { get; set; }
    public Guid ItemId { get; set; }
    public string? CurrentUserId { get; set; }
}