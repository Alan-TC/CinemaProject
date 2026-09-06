using CinemaBackend.Domain.Storage.Movie;
using Moq;

namespace TestsCommonUtilities.Storage.Movie;

public class IMovieImageStorageBuilder
{
    private readonly Mock<IMovieImageStorage> _mock = new();

    public IMovieImageStorage Build() => _mock.Object;
}
