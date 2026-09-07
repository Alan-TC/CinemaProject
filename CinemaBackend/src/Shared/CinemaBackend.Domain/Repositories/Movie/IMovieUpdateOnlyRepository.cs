namespace CinemaBackend.Domain.Repositories.Movie;

public interface IMovieUpdateOnlyRepository
{
    Task<Entities.Movie?> GetMovieById(Guid id);
}
