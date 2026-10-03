using CinemaBackend.Domain.Security.PasswordHasher;
using CinemaBackend.Domain.Security.Tokens.Admin;
using CinemaBackend.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestsCommonUtilities;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Identity;

namespace Admin.WebApi.Tests;

public class AdminWebApplicationFactory : CommonWebApplicationFactory<Program>
{
    private readonly IConfiguration _configuration;
    public AdminUserIdentityManager AdminUser { get; private set; } = default!;
    public AdminWebApplicationFactory()
    {
        var config = new Dictionary<string, string?> 
            {
                { "Jwt:SigningKey", "TEST-ADMIN--JWT-KEY-WITH-32-CHAR" },
                { "Jwt:Issuer", "CinemaBackendAdmin" },
                { "Jwt:Audience", "CinemaBackendAdmin" },
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
        var adminAccessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAdminAccessTokenGenerator>();
        var adminRefresTokenGenerator = scope.ServiceProvider.GetRequiredService<IAdminRefreshTokenGenerator>();

        AdminUser = await CreateAdminUser(dbContext, passwordHasher, adminAccessTokenGenerator, adminRefresTokenGenerator);
    }

    private static async Task<AdminUserIdentityManager> CreateAdminUser(CinemaDbContext dbContext,
    IPasswordHasher passwordHasher, IAdminAccessTokenGenerator adminAccessTokenGenerator,
    IAdminRefreshTokenGenerator adminRefreshTokenGenerator)
    {
        var (adminUser, password) = new AdminUserBuilder()
            .WithPasswordHasher(passwordHasher)
            .Build();

        var accessToken = adminAccessTokenGenerator.Generate(adminUser);
        var refreshToken = adminRefreshTokenGenerator.Generate(adminUser);

        await dbContext.AdminUsers.AddAsync(adminUser);
        await dbContext.SaveChangesAsync();

        return new AdminUserIdentityManager(adminUser, password, accessToken, refreshToken);
    }
}
