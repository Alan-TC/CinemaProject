using Bogus;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class RegisterUserAdminRequestJsonBuilder
{
    private readonly Faker<RegisterUserAdminRequestJson> _faker;

    public RegisterUserAdminRequestJsonBuilder()
    {
        _faker = new Faker<RegisterUserAdminRequestJson>();

        _faker.RuleFor(r => r.Email, f => f.Internet.Email());
        _faker.RuleFor(r => r.Password, f => f.Internet.Password());
    }

    public RegisterUserAdminRequestJsonBuilder WithPassword(string password)
    {
        _faker.RuleFor(r => r.Password, password);

        return this;
    }

    public RegisterUserAdminRequestJson Build() => _faker.Generate();
}
