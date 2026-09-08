namespace CinemaBackend.SharedKernel.Pagination;

public class PaginationReturn<T>(List<T> items, int currentPage, int pageSize, int totalCount)
{
    public int CurrentPage { get; set; } = currentPage;
    public int TotalPages { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalCount;

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public List<T> Items { get; set; } = items;
}
