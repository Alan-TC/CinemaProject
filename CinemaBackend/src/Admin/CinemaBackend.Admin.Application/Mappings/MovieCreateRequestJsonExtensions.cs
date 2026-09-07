using CinemaBackend.Communication.Requests;
using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Admin.Application.Mappings;

internal static class MovieRequestJsonExtensions
{
    internal static Movie MapToMovie(this MovieRequestJson request)
    {
        return new Movie
        {
          Title = request.Title,
          Sinopse = request.Sinopse,
          Duration = request.Duration,
          ReleaseDate = request.ReleaseDate,
          FilmGenres = [.. request.FilmGenres
            .Select(genre => new FilmGenre {
                    Genre = (Genres)genre
                }
            )],
          ClassInd = (ClassInd)request.ClassInd,
        };
    }
}
