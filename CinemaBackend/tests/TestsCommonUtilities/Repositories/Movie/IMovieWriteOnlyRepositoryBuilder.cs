using CinemaBackend.Domain.Repositories.Movie;
using Moq;

namespace TestsCommonUtilities.Repositories.Movie;

public class IMovieWriteOnlyRepositoryBuilder
{
    private readonly Mock<IMovieWriteOnlyRepository> _mock = new ();

    public IMovieWriteOnlyRepository Build() => _mock.Object;
}
