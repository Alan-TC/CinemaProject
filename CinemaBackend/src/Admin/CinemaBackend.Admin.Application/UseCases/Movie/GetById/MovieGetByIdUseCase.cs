using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Admin.Application.UseCases.Movie.GetById;

public class MovieGetByIdUseCase(
    IMovieReadOnlyRepository movieReadOnlyRepository,
    IMovieImageStorage movieImageStorage
) 
    : IMovieGetByIdUseCase
{
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository = movieReadOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;

    public async Task<MovieResponseJson> Execute(Guid movieId)
    {
        var movie = await _movieReadOnlyRepository.GetMovieById(movieId);

        if (movie == null)
            throw new NotFoundException();

        var movieUrls = _movieImageStorage.GetMovieImagesUrl(movieId);

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
