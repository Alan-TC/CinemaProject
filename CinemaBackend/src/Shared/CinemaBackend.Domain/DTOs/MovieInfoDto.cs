using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.DTOs;

public class MovieInfoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Sinopse { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public ICollection<Genres> FilmGenres { get; set; } = [];
    public ClassInd ClassInd { get; set; }
    public MovieImagesUrlDto? ImagesUrl { get; set; }
}
