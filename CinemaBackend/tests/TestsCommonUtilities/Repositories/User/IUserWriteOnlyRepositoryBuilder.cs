using CinemaBackend.Domain.Repositories.User;
using Moq;

namespace TestsCommonUtilities.Repositories.User;

public class IUserWriteOnlyRepositoryBuilder
{
    private readonly Mock<IUserWriteOnlyRepository> _mock = new();    

    public IUserWriteOnlyRepository Build() => _mock.Object;
}
