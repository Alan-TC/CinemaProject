using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.Entities.MovieEntities;

public class Movie : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string Sinopse { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public ICollection<FilmGenre> FilmGenres { get; set; } = [];
    public ClassInd ClassInd { get; set; }
}
