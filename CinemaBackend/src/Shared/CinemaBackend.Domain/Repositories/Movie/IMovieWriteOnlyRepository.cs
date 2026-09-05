namespace CinemaBackend.Domain.Repositories.Movie;

public interface IMovieWriteOnlyRepository
{
    Task Add(Entities.Movie movie);
}
