using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Admin.Application.UseCases.Movie.UpdateDetails;

public interface IUpdateMovieDetailsUseCase
{
    Task Execute(Guid movieId, MovieRequestJson request);
}
