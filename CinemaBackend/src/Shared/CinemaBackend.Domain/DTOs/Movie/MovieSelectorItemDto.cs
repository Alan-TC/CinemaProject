namespace CinemaBackend.Domain.DTOs.Movie;

public class MovieSelectorItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}
