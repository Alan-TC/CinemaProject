using CinemaBackend.Communication.Enums;

namespace CinemaBackend.Communication.Requests;

public class MovieRequestJson
{
    public string Title { get; set; } = string.Empty;
    public string Sinopse { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<Genres> FilmGenres { get; set; } = [];
    public ClassInd ClassInd { get; set; }
}
