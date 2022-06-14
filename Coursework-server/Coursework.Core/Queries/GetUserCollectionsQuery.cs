using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetUserCollectionsQuery : IRequest<List<ShortCollectionVm>>
{
    public Guid UserId { get; }

    public GetUserCollectionsQuery(Guid userId)
    {
        UserId = userId;
    }
}