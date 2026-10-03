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

    public IUserReadOnlyRepository Build() => _mock.Object;
}
