namespace Coursework.Core.Pagination;

public interface IPaginationParams
{
    int Page { get; set; }
    int Size { get; set; }
}