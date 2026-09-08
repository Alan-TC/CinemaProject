namespace CinemaBackend.SharedKernel.Pagination;

public class PaginationQuery(int currentPage, int pageSize = 10, int maxPageSize = 50)
{
    private readonly int _maxPageSize = maxPageSize;
    private readonly int _pageSize = (pageSize < 10) ? 10 : pageSize;
    public int CurrentPage { get; private set; } = Math.Max(1, currentPage);
    public int PageSize => Math.Min(_pageSize, _maxPageSize);
}
