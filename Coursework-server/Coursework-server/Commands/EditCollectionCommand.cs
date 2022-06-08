#nullable enable
using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Commands;

public class EditCollectionCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? CoverUrl { get; set; }
    public Guid TopicId { get; set; }
    public Guid OwnerId { get; set; }
    public List<FieldVm> FieldVMs { get; set; } = new List<FieldVm>();
    public string? CurrentUserId { get; set; }
}