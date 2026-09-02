using Bogus;
using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Security.PasswordHasher;
using TestsCommonUtilities.Security;

namespace TestsCommonUtilities.Entities;

public class AdminUserBuilder
{
    private readonly Faker<AdminUser> _faker = new();

    private string? _password;
    private IPasswordHasher? _passwordHasher;

    public AdminUserBuilder()
    {
        _faker.RuleFor(u => u.Email, f => f.Internet.Email());
        _faker.RuleFor(u => u.PasswordHash, f => f.Internet.Password());
    }

    public AdminUserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public AdminUserBuilder WithPasswordHasher(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        return this;
    }

    private string HashedPassword(string password)
    {
        var hasher = _passwordHasher ?? new IPasswordHasherBuilder().Build();

        return hasher.HashPassword(password);
    }

    public (AdminUser user, string password) Build() 
    {
        var password = _password ?? new Faker().Internet.Password();

        _faker.RuleFor(u => u.PasswordHash, HashedPassword(password));

        return (_faker.Generate(), password);
    }
}
