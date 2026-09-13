using CinemaBackend.Domain.Repositories.Session;
using Moq;

namespace TestsCommonUtilities.Repositories.Sessions;

public class ISessionReadOnlyRepositoryBuilder
{
    private readonly Mock<ISessionReadOnlyRepository> _mock = new();

    public ISessionReadOnlyRepository Build() => _mock.Object;
}
