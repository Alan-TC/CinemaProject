using CinemaBackend.Domain.Repositories.Session;
using Moq;

namespace TestsCommonUtilities.Repositories.Sessions;

public class ISessionWriteOnlyRepositoryBuilder
{
    private readonly Mock<ISessionWriteOnlyRepository> _mock = new();

    public ISessionWriteOnlyRepository Build() => _mock.Object;
}
