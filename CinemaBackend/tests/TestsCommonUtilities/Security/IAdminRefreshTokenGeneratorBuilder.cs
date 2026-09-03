using Bogus;
using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Security.Tokens.Admin;
using Moq;

namespace TestsCommonUtilities.Security;

public class IAdminRefreshTokenGeneratorBuilder
{
    private readonly Mock<IAdminRefreshTokenGenerator> _mock = new();

    public IAdminRefreshTokenGeneratorBuilder()
    {
        var fakeToken = new Faker().Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        
        _mock.Setup(m => m.Generate(It.IsAny<AdminUser>())).Returns(fakeToken);
    }

    public IAdminRefreshTokenGenerator Build() => _mock.Object;
}
