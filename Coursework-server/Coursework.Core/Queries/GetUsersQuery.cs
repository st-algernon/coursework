using Coursework.Core.Data.ViewModels;
using Coursework.Core.Pagination;
using MediatR;

namespace Coursework.Core.Queries;

public class GetUsersQuery : IRequest<List<UserVm>>, IPaginationParams
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