using Bogus;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class UserRequestJsonBuilder
{
    private readonly Faker<UserRequestJson> _faker = new ();

    public UserRequestJsonBuilder()
    {
        _faker.RuleFor(r => r.Name, f => f.Name.FirstName());
        _faker.RuleFor(r => r.Email, (f, r) => f.Internet.Email(r.Name));
        _faker.RuleFor(r => r.Password, f => f.Internet.Password());
    }

    public UserRequestJsonBuilder WithName(string name)
    {
        _faker.RuleFor(r => r.Name, name);
        return this;
    }

    public UserRequestJsonBuilder WithPasswordLength(int length)
    {
        _faker.RuleFor(r => r.Password, f => f.Internet.Password(length));
        return this;
    }

    public UserRequestJsonBuilder WithPassword(string password)
    {
        _faker.RuleFor(r => r.Password, password);
        return this;
    }

    public UserRequestJsonBuilder WithEmail(string email)
    {
        _faker.RuleFor(r => r.Email, email);
        return this;
    }

    public UserRequestJson Build() => _faker.Generate();
}
