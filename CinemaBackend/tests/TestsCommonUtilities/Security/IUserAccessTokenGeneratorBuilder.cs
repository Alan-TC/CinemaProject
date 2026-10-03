using Bogus;
using CinemaBackend.Domain.Entities.UserEntities;
using CinemaBackend.Domain.Security.Tokens.User;
using Moq;

namespace TestsCommonUtilities.Security;

public class IUserAccessTokenGeneratorBuilder
{
    private readonly Mock<IUserAccessTokenGenerator> _mock = new();

    public IUserAccessTokenGeneratorBuilder()
    {
        var fakeToken = new Faker().Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        
        _mock.Setup(m => m.Generate(It.IsAny<User>())).Returns(fakeToken);
    }

    public IUserAccessTokenGenerator Build() => _mock.Object;
}
