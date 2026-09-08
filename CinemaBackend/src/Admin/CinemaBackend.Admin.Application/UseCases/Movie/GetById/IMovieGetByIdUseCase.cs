using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Admin.Application.UseCases.Movie.GetById;

public interface IMovieGetByIdUseCase
{
    Task<MovieResponseJson> Execute(Guid movieId);
}
