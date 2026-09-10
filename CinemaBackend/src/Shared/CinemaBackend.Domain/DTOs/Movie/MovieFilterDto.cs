using CinemaBackend.Domain.DTOs.Shared;
using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.DTOs.Movie;

public record MovieFilterDto
{
    public string? SearchTerm = null;
    public IList<Genres> Genres { get; set;} = [];
    public DateBetweenDto? ReleaseDate = null;
    public ClassInd? ClassInd = null;
};
