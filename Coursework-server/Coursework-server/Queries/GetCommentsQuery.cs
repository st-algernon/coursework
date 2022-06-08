using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetCommentsQuery : IRequest<List<CommentVm>>
{
    public Guid ItemId { get; }

    public GetCommentsQuery(Guid itemId)
    {
        ItemId = itemId;
    }
}