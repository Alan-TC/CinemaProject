using System.Reflection;
using CinemaBackend.Domain.Repositories;
using CinemaBackend.Domain.Security.PasswordHasher;
using CinemaBackend.Domain.Security.Tokens;
using CinemaBackend.Infrastructure.Data;
using CinemaBackend.Infrastructure.Data.Repositories;
using CinemaBackend.Infrastructure.Security.PasswordHasher;
using CinemaBackend.Infrastructure.Security.Tokens;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBackend.Infrastructure;

public static class DependencyInjectionExtensions
{
    private const string DbConnectionName = "DbConnection";
    public static void AddAdminInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddCommonInfrastructure(services, configuration);

        services.AddScoped<IAdminUserReadOnlyRepository, AdminUserRepository>();
    }

    public static void AddClientInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddCommonInfrastructure(services, configuration);
    }

    private static void AddCommonInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddMigrations(services, configuration);

        services.AddScoped<IPasswordHasher, PasswordHasherArgon2id>();
        AddJwtTokenHandler(services, configuration);
    }

    private static void AddJwtTokenHandler(this IServiceCollection services, IConfiguration configuration)
    {
        var signingKey = configuration["Jwt:SigningKey"]!;
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var accessTokenExpirationMinutes = uint.Parse(configuration["Jwt:AccessTokenExpirationTimeMinutes"]!);
        var refreshTokenExpirationMinutes = uint.Parse(configuration["Jwt:RefreshTokenExpirationTimeMinutes"]!);

        services.AddScoped<IAccessTokenGenerator>(provider =>
            new JwtTokenHandler(signingKey, issuer, audience, accessTokenExpirationMinutes, refreshTokenExpirationMinutes));

        services.AddScoped<IRefreshTokenGenerator>(provider =>
            new JwtTokenHandler(signingKey, issuer, audience, accessTokenExpirationMinutes, refreshTokenExpirationMinutes));
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CinemaDbContext>(config =>
        {
            var connectionString = configuration.GetConnectionString(DbConnectionName);
            config.UseNpgsql(connectionString, options =>
            {
                options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "public");
            })
            .UseSnakeCaseNamingConvention();
        });
    }

    private static void AddMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore().ConfigureRunner(config =>
        {
            config.AddPostgres()
            .WithGlobalConnectionString(_ =>
            {
                var connectionString = configuration.GetConnectionString(DbConnectionName);
                return connectionString;
            })
            .ScanIn(Assembly.Load("CinemaBackend.Infrastructure"))
            .For.All();
        });
    }
}
