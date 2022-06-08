using Coursework_server.Data.ViewModels;
using Coursework_server.Pagination;
using MediatR;

namespace Coursework_server.Queries;

public class GetLastAddedItemsQuery : IRequest<List<ShortItemVm>>, IPaginationParams
{
    private const int MaxSize = 50;
    private int _size = 15;
    public int Page { get; set; } = 1;
    public int Size
    {
        get => _size;
        set => _size = (value > MaxSize) ? MaxSize : value;
    }
}