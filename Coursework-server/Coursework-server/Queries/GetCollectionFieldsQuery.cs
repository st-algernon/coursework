using Coursework_server.Data.ViewModels;
using MediatR;

namespace Coursework_server.Queries;

public class GetCollectionFieldsQuery : IRequest<List<FieldWithTypeNameVm>>
{
    public Guid Id { get; }

    public GetCollectionFieldsQuery(Guid id)
    {
        Id = id;
    }
}