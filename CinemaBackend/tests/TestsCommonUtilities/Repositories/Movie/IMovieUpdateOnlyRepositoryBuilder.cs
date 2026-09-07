using CinemaBackend.Domain.Repositories.Movie;
using Moq;

namespace TestsCommonUtilities.Repositories.Movie;

public class IMovieUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IMovieUpdateOnlyRepository> _mock = new();

    public IMovieUpdateOnlyRepositoryBuilder GetMovieById(CinemaBackend.Domain.Entities.Movie movie)
    {
        _mock.Setup(r => r.GetMovieById(movie.Id)).ReturnsAsync(movie);

        return this;
    }

    public IMovieUpdateOnlyRepository Build() => _mock.Object;
}
