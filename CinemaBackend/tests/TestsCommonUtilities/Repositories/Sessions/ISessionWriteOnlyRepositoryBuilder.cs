using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Repositories.Session;
using Moq;

namespace TestsCommonUtilities.Repositories.Sessions;

public class ISessionWriteOnlyRepositoryBuilder
{
    private readonly Mock<ISessionWriteOnlyRepository> _mock = new();

    public ISessionWriteOnlyRepositoryBuilder GetById(Session session)
    {
        _mock.Setup(r => r.GetById(session.Id)).ReturnsAsync(session);
        return this;
    }

    public ISessionWriteOnlyRepository Build() => _mock.Object;
}
