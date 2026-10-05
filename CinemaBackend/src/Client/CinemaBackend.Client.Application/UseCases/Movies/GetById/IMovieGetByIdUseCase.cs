using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Client.Application.UseCases.Movies.GetById;

public interface IMovieGetByIdUseCase
{
    Task<MovieResponseJson> Execute(Guid movieId);
}
