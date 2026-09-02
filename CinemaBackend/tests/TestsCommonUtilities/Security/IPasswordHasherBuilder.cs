using CinemaBackend.Domain.Security.PasswordHasher;
using Moq;

namespace TestsCommonUtilities.Security;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _mock = new();
    public IPasswordHasherBuilder()
    {
        _mock.Setup(passwordHasher => passwordHasher.HashPassword(It.IsAny<string>())).Returns(() => $"hashed_password");
    }

    public IPasswordHasherBuilder VerifyPassword(string password)
    {
        _mock.Setup(passwordHasher => passwordHasher.VerifyPassword(password, It.IsAny<string>())).Returns(true);

        return this;
    }

    public IPasswordHasher Build() => _mock.Object;
}
