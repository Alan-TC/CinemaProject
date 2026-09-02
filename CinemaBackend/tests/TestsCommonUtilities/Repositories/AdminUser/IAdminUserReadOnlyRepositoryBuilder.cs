using CinemaBackend.Domain.Repositories.AdminUser;
using Moq;

namespace TestsCommonUtilities.Repositories.AdminUser;

public class IAdminUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IAdminUserReadOnlyRepository> _mock = new();

    public IAdminUserReadOnlyRepositoryBuilder AdminUserEmailExists(string email)
    {
        _mock.Setup(repository => repository.AdminUserEmailExists(email)).ReturnsAsync(true);

        return this;
    }

    public IAdminUserReadOnlyRepositoryBuilder GetAdminUserByEmail(CinemaBackend.Domain.Entities.AdminUser adminUser)
    {
        _mock.Setup(repository => repository.GetAdminUserByEmail(adminUser.Email)).ReturnsAsync(adminUser);

        return this;
    }

    public IAdminUserReadOnlyRepository Build() => _mock.Object;
}
