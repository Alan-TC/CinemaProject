namespace CinemaBackend.Communication.Requests;

public class PaginationQueryRequest
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}
