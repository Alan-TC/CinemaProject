using CinemaBackend.Domain.Security.PasswordHasher;
using CinemaBackend.Domain.Security.Tokens;
using CinemaBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Resources;

namespace TestsCommonUtilities;

public abstract class CommonWebApplicationFactory<TEntryPoint> : 
    WebApplicationFactory<TEntryPoint>, IAsyncLifetime where TEntryPoint : class
{
    public AdminUserIdentityManager AdminUser { get; private set; } = default!;

    private readonly PostgreSqlContainer _postgreSqlContainer;

    public CommonWebApplicationFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder("postgres:17")
            .WithDatabase("cinema_development_db")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
        var refresTokenGenerator = scope.ServiceProvider.GetRequiredService<IRefreshTokenGenerator>();

        AdminUser = await CreateAdminUser(dbContext, passwordHasher, accessTokenGenerator, refresTokenGenerator);
    }

    private static async Task<AdminUserIdentityManager> CreateAdminUser(CinemaDbContext dbContext, 
        IPasswordHasher passwordHasher, IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        var (adminUser, password) = new AdminUserBuilder()
            .WithPasswordHasher(passwordHasher)
            .Build();

        var accessToken = accessTokenGenerator.Generate(adminUser.Id.ToString());
        var refreshToken = refreshTokenGenerator.Generate(adminUser.Id.ToString());

        await dbContext.AdminUsers.AddAsync(adminUser);
        await dbContext.SaveChangesAsync();

        return new AdminUserIdentityManager(adminUser, password, accessToken, refreshToken);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(GetConfiguration())
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DbConnection"] = _postgreSqlContainer.GetConnectionString()
                };

                configuration.AddInMemoryCollection(parameters);
            });
    }

    protected abstract IConfiguration GetConfiguration();

    Task IAsyncLifetime.DisposeAsync() => _postgreSqlContainer.StopAsync();
}
