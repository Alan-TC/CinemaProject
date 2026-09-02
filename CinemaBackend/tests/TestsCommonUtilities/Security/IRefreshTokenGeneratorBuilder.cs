using Bogus;
using CinemaBackend.Domain.Security.Tokens;
using Moq;

namespace TestsCommonUtilities.Security;

public class IRefreshTokenGeneratorBuilder
{
    private readonly Mock<IRefreshTokenGenerator> _mock = new();

    public IRefreshTokenGeneratorBuilder()
    {
        var fakeToken = new Faker().Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        
        _mock.Setup(m => m.Generate(It.IsAny<string>())).Returns(fakeToken);
    }

    public IRefreshTokenGenerator Build() => _mock.Object;
}
