using Coursework.Core.Data.ViewModels;
using MediatR;

namespace Coursework.Core.Queries;

public class GetCollectionFieldsQuery : IRequest<List<FieldWithTypeNameVm>>
{
    public Guid Id { get; }

    public GetCollectionFieldsQuery(Guid id)
    {
        Id = id;
    }
}