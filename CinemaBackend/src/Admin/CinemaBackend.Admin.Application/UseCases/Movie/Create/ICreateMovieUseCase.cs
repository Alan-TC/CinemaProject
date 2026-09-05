namespace CinemaBackend.Admin.Application.UseCases.Movie.Create;

public interface ICreateMovieUseCase
{
    Task Execute(CreateMovieUseCaseRequest request);
}
