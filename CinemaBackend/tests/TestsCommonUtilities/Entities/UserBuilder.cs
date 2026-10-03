using Bogus;
using CinemaBackend.Domain.Entities.UserEntities;
using CinemaBackend.Domain.Security.PasswordHasher;
using TestsCommonUtilities.Security;

namespace TestsCommonUtilities.Entities;

public class UserBuilder
{
    private readonly Faker<User> _faker = new();

    private string? _password;
    private IPasswordHasher? _passwordHasher;

    public UserBuilder()
    {
        _faker.RuleFor(u => u.Name, f => f.Name.FirstName());
        _faker.RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name));
        _faker.RuleFor(u => u.PasswordHash, f => f.Internet.Password());
    }

    public UserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserBuilder WithPasswordHasher(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        return this;
    }

    private string HashedPassword(string password)
    {
        var hasher = _passwordHasher ?? new IPasswordHasherBuilder().Build();

        return hasher.HashPassword(password);
    }

    public (User user, string password) Build() 
    {
        var password = _password ?? new Faker().Internet.Password();

        _faker.RuleFor(u => u.PasswordHash, HashedPassword(password));

        return (_faker.Generate(), password);
    }
}
