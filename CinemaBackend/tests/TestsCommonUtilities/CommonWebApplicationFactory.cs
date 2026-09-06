using CinemaBackend.Domain.Security.PasswordHasher;
using CinemaBackend.Domain.Security.Tokens.Admin;
using CinemaBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Identity;

namespace TestsCommonUtilities;

public abstract class CommonWebApplicationFactory<TEntryPoint> : 
    WebApplicationFactory<TEntryPoint>, IAsyncLifetime where TEntryPoint : class
{
    public AdminUserIdentityManager AdminUser { get; private set; } = default!;

    private readonly PostgreSqlContainer _postgreSqlContainer;
    private readonly string _storagePath;

    public CommonWebApplicationFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder("postgres:17")
            .WithDatabase("cinema_development_db")
            .Build();
        
        _storagePath = Path.Combine(Environment.CurrentDirectory, Guid.CreateVersion7().ToString());
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        Directory.CreateDirectory(_storagePath);

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

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(GetConfiguration())
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DbConnection"] = _postgreSqlContainer.GetConnectionString(),
                    ["Host:StorageRootPath"] = _storagePath,
                    ["Host:StorageRequestPath"] = "assets" 
                };

                configuration.AddInMemoryCollection(parameters);
            });
    }

    protected abstract IConfiguration GetConfiguration();

    Task IAsyncLifetime.DisposeAsync() {
        Directory.Delete(_storagePath, true);

        return _postgreSqlContainer.StopAsync();
    }
}
