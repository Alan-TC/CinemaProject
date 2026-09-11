using CinemaBackend.Admin.Application.Mappings;
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

        return movie.MapToMovieResponseJson(_movieImageStorage);
    }
}
