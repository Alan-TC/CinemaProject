using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Admin.Application.Mappings;

public static class Movie
{
    public static MovieResponseJson MapToMovieResponseJson(this Domain.Entities.MovieEntities.Movie movie, IMovieImageStorage movieImageStorage)
    {
        var movieUrls = movieImageStorage.GetMovieImagesUrl(movie.Id);

        return new MovieResponseJson
        {
            Id = movie.Id,
            Title = movie.Title,
            Sinopse = movie.Sinopse,
            Duration = movie.Duration,
            ReleaseDate = movie.ReleaseDate,
            ClassInd = (Communication.Enums.ClassInd)movie.ClassInd,
            FilmGenres = [.. movie.FilmGenres.Select(g => (Communication.Enums.Genres)g.Genre)],
            ImagesUrl = new MovieImagesUrlResponseJson
            {
                PosterUrl = movieUrls.PosterUrl,
                FeaturedBanner = movieUrls.FeaturedBanner,
                StandardBanner = movieUrls.StandardBanner,
            }
        };
    }
}
