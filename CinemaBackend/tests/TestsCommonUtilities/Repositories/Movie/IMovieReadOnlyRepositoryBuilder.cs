using CinemaBackend.Domain.Repositories.Movie;
using Moq;

namespace TestsCommonUtilities.Repositories.Movie;

public class IMovieReadOnlyRepositoryBuilder
{
    private readonly Mock<IMovieReadOnlyRepository> _mock = new();

    public IMovieReadOnlyRepositoryBuilder GetMovieById(CinemaBackend.Domain.Entities.MovieEntities.Movie movie)
    {
        _mock.Setup(r => r.GetMovieById(movie.Id)).ReturnsAsync(movie);

        return this;
    }

    public IMovieReadOnlyRepository Build() => _mock.Object;
}
