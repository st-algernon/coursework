using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Commands;

public class CreateItemCommand : IRequest<Unit>
{
    public string Title { get; set; }
    public string? CoverUrl { get; set; }
    public Guid CollectionId { get; set; }
    public List<string> TagNames { get; set; } = new List<string>();
    public List<FullFieldVm> FullFieldVMs { get; set; } = new List<FullFieldVm>();
    public string? CurrentUserId { get; set; }
}