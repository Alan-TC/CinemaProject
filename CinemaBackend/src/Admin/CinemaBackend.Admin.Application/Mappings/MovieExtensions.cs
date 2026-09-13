using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Entities.MovieEntities;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Admin.Application.Mappings;

public static class MovieExtensions
{
    public static MovieResponseJson MapToMovieResponseJson(this Movie entity, IMovieImageStorage movieImageStorage)
    {
        var movieUrls = movieImageStorage.GetMovieImagesUrl(entity.Id);

        return new MovieResponseJson
        {
            Id = entity.Id,
            Title = entity.Title,
            Sinopse = entity.Sinopse,
            Duration = entity.Duration,
            ReleaseDate = entity.ReleaseDate,
            ClassInd = (Communication.Enums.ClassInd)entity.ClassInd,
            FilmGenres = [.. entity.FilmGenres.Select(g => (Communication.Enums.Genres)g.Genre)],
            ImagesUrl = new MovieImagesUrlResponseJson
            {
                PosterUrl = movieUrls.PosterUrl,
                FeaturedBanner = movieUrls.FeaturedBanner,
                StandardBanner = movieUrls.StandardBanner,
            }
        };
    }
}
