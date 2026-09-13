using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Admin.Application.UseCases.Movie.Create;

public interface ICreateMovieUseCase
{
    Task<MovieResponseJson> Execute(CreateMovieUseCaseRequest request);
}
