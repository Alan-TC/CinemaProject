using Bogus;
using CinemaBackend.Domain.Security.Tokens;
using Moq;

namespace TestsCommonUtilities.Security;

public class IAccessTokenGeneratorBuilder
{
    private readonly Mock<IAccessTokenGenerator> _mock = new();

    public IAccessTokenGeneratorBuilder()
    {
        var fakeToken = new Faker().Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        
        _mock.Setup(m => m.Generate(It.IsAny<string>())).Returns(fakeToken);
    }

    public IAccessTokenGenerator Build() => _mock.Object;
}
