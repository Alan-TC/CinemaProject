namespace CinemaBackend.Domain.Repositories.Movie;

public interface IMovieReadOnlyRepository
{
    Task<Entities.Movie?> GetMovieById(Guid id);
}
