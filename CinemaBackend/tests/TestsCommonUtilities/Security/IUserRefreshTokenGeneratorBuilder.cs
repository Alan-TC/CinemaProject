using Bogus;
using CinemaBackend.Domain.Entities.UserEntities;
using CinemaBackend.Domain.Security.Tokens.User;
using Moq;

namespace TestsCommonUtilities.Security;

public class IUserRefreshTokenGeneratorBuilder
{
    private readonly Mock<IUserRefreshTokenGenerator> _mock = new();

    public IUserRefreshTokenGeneratorBuilder()
    {
        var fakeToken = new Faker().Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        
        _mock.Setup(m => m.Generate(It.IsAny<User>())).Returns(fakeToken);
    }

    public IUserRefreshTokenGenerator Build() => _mock.Object;
}
