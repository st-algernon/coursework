using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetCommentsQuery : IRequest<List<CommentVm>>
{
    public Guid ItemId { get; }

    public GetCommentsQuery(Guid itemId)
    {
        ItemId = itemId;
    }
}