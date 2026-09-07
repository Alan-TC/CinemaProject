using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Admin.Application.UseCases.Movie.UpdateImages;

public interface IMovieUpdateImagesUseCase
{
    Task Execute(MovieImagesRequestJson request);
}
