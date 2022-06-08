namespace Coursework_server.Pagination;

public interface IPaginationParams
{
    int Page { get; set; }
    int Size { get; set; }
}