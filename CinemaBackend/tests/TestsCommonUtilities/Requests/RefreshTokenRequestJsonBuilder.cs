using Bogus;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class RefreshTokenRequestJsonBuilder
{
    private readonly Faker<RefreshTokenRequestJson> _faker = new();

    public RefreshTokenRequestJsonBuilder()
    {
        _faker.RuleFor(r => r.RefreshToken, f => f.Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"));
    }

    public RefreshTokenRequestJsonBuilder WithRefreshToken(string refreshToken)
    {
        _faker.RuleFor(r => r.RefreshToken, refreshToken);
        return this;
    }

    public RefreshTokenRequestJson Build() => _faker.Generate();
}
