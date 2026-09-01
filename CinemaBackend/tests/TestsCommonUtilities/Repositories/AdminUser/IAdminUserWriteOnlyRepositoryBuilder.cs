using CinemaBackend.Domain.Repositories.AdminUser;
using Moq;

namespace TestsCommonUtilities.Repositories.AdminUser;

public class IAdminUserWriteOnlyRepositoryBuilder
{
    private readonly Mock<IAdminUserWriteOnlyRepository> _mock = new();    

    public IAdminUserWriteOnlyRepository Build() => _mock.Object;
}
