namespace CinemaBackend.Communication.Responses;

public class MovieSelectorItemResponseJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}
