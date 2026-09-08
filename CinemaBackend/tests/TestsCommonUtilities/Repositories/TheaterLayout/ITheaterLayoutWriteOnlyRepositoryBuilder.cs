using CinemaBackend.Domain.Repositories.TheaterLayout;
using Moq;

namespace TestsCommonUtilities.Repositories.TheaterLayout;

public class ITheaterLayoutWriteOnlyRepositoryBuilder
{
    private readonly Mock<ITheaterLayoutWriteOnlyRepository> _mock = new();

    public ITheaterLayoutWriteOnlyRepository Build() => _mock.Object;
}
