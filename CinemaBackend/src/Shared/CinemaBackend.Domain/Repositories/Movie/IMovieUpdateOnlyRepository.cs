namespace CinemaBackend.Domain.Repositories.Movie;

public interface IMovieUpdateOnlyRepository
{
    Task<Entities.MovieEntities.Movie?> GetMovieById(Guid id);
}
