using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.Entities;

public class FilmGenre : EntityBase
{
    public Genres Genre { get; set; }
    public Guid MovieId { get; private set; }
}
