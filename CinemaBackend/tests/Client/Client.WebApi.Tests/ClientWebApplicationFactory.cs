using CinemaBackend.Domain.Security.PasswordHasher;
using CinemaBackend.Domain.Security.Tokens.Admin;
using CinemaBackend.Domain.Security.Tokens.User;
using CinemaBackend.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestsCommonUtilities;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Identity;

namespace Client.WebApi.Tests;

public class ClientWebApplicationFactory : CommonWebApplicationFactory<Program>
{
    private readonly IConfiguration _configuration;
    public UserIdentityManager User { get; private set; } = default!;

    public ClientWebApplicationFactory()
    {
        var config = new Dictionary<string, string?>
            {
                { "Jwt:SigningKey", "TEST-CLIEN--JWT-KEY-WITH-32-CHAR" },
                { "Jwt:Issuer", "CinemaBackendClient" },
                { "Jwt:Audience", "CinemaBackendClient" },
                { "Jwt:AccessTokenExpirationTimeMinutes", "1000" },
                { "Jwt:RefreshTokenExpirationTimeMinutes", "3600" }
            };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
    }

    protected override IConfiguration GetConfiguration() => _configuration;

    protected override async Task InitializeDatabase()
    {
        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var userAccessTokenGenerator = scope.ServiceProvider.GetRequiredService<IUserAccessTokenGenerator>();
        var userRefreshTokenGenerator = scope.ServiceProvider.GetRequiredService<IUserRefreshTokenGenerator>();

        User = await CreateUser(dbContext, passwordHasher, userAccessTokenGenerator, userRefreshTokenGenerator);
    }

    private static async Task<UserIdentityManager> CreateUser(
        CinemaDbContext dbContext, 
        IPasswordHasher passwordHasher,
        IUserAccessTokenGenerator userAccessTokenGenerator,
        IUserRefreshTokenGenerator userRefreshTokenGenerator
        )
    {
        var (user, password) = new UserBuilder()
            .WithPasswordHasher(passwordHasher)
            .Build();

        var accessToken = userAccessTokenGenerator.Generate(user);
        var refreshToken = userRefreshTokenGenerator.Generate(user);

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return new UserIdentityManager(user, password, accessToken, refreshToken);
    }
}
