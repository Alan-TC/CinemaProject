using CinemaBackend.Domain.Repositories.User;
using Moq;

namespace TestsCommonUtilities.Repositories.User;

public class IUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock = new();

    public IUserReadOnlyRepositoryBuilder UserEmailExists(string email)
    {
        _mock.Setup(repository => repository.UserEmailExists(email)).ReturnsAsync(true);

        return this;
    }

    public IUserReadOnlyRepositoryBuilder GetUserByEmail(CinemaBackend.Domain.Entities.UserEntities.User user)
    {
        _mock.Setup(repository => repository.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}
