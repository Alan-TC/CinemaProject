using Bogus;
using CinemaBackend.Domain.Entities.AdminUserEntities;
using CinemaBackend.Domain.Security.Tokens.Admin;
using Moq;

namespace TestsCommonUtilities.Security;

public class IAdminAccessTokenGeneratorBuilder
{
    private readonly Mock<IAdminAccessTokenGenerator> _mock = new();

    public IAdminAccessTokenGeneratorBuilder()
    {
        var fakeToken = new Faker().Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        
        _mock.Setup(m => m.Generate(It.IsAny<AdminUser>())).Returns(fakeToken);
    }

    public IAdminAccessTokenGenerator Build() => _mock.Object;
}
