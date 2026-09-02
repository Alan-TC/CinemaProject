using Bogus;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class LoginRequestJsonBuilder
{
    private readonly Faker<LoginRequestJson> _faker = new();

    public LoginRequestJsonBuilder()
    {
        _faker.RuleFor(r => r.Email, f => f.Internet.Email());
        _faker.RuleFor(r => r.Password, f => f.Internet.Password());
    }

    public LoginRequestJsonBuilder WithEmail(string email)
    {
        _faker.RuleFor(r => r.Email, email);

        return this;
    }

    public LoginRequestJsonBuilder WithPassword(string password)
    {
        _faker.RuleFor(r => r.Password, password);

        return this;
    }

    public LoginRequestJson Build() => _faker.Generate();
}
