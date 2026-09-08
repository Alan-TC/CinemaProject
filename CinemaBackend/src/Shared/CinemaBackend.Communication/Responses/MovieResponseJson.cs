using CinemaBackend.Communication.Enums;

namespace CinemaBackend.Communication.Responses;

public class MovieResponseJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Sinopse { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public ICollection<Genres> FilmGenres { get; set; } = [];
    public ClassInd ClassInd { get; set; }
    public MovieImagesUrlResponseJson ImagesUrl { get; set; } = default!;
}
