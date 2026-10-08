using System.Reflection;
using CinemaBackend.Domain.Identity.Client;
using CinemaBackend.Domain.Repositories.AdminUser;
using CinemaBackend.Domain.Repositories.CinemaTicket;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.Domain.Repositories.Payment;
using CinemaBackend.Domain.Repositories.Session;
using CinemaBackend.Domain.Repositories.TheaterLayout;
using CinemaBackend.Domain.Repositories.User;
using CinemaBackend.Domain.Security.PasswordHasher;
using CinemaBackend.Domain.Security.Tokens.Admin;
using CinemaBackend.Domain.Security.Tokens.User;
using CinemaBackend.Domain.Storage.Movie;
using CinemaBackend.Infrastructure.Data;
using CinemaBackend.Infrastructure.Data.Repositories;
using CinemaBackend.Infrastructure.Identity;
using CinemaBackend.Infrastructure.Security.PasswordHasher;
using CinemaBackend.Infrastructure.Security.Tokens;
using CinemaBackend.Infrastructure.Storage;
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
        AddAdminJwtTokenHandler(services, configuration);

        services.AddScoped<IAdminUserReadOnlyRepository, AdminUserRepository>();
        services.AddScoped<IAdminUserWriteOnlyRepository, AdminUserRepository>();

        services.AddScoped<IMovieImageStorage, MovieImageStorage>();
        services.AddScoped<IMovieWriteOnlyRepository, MovieRepository>();
        services.AddScoped<IMovieReadOnlyRepository, MovieRepository>();
        services.AddScoped<IMovieUpdateOnlyRepository, MovieRepository>();

        services.AddScoped<ITheaterLayoutReadOnlyRepository, TheaterLayoutRepository>();
        services.AddScoped<ITheaterLayoutWriteOnlyRepository, TheaterLayoutRepository>();

        services.AddScoped<ISessionReadOnlyRepository, SessionRepository>();
        services.AddScoped<ISessionWriteOnlyRepository, SessionRepository>();
    }

    public static void AddClientInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddCommonInfrastructure(services, configuration);
        AddClientJwtTokenHandler(services, configuration);

        services.AddScoped<IUserReadOnlyRepository,  UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository,  UserRepository>();

        services.AddScoped<IMovieImageStorage, MovieImageStorage>();
        services.AddScoped<IMovieReadOnlyRepository, MovieRepository>();

        services.AddScoped<ISessionReadOnlyRepository, SessionRepository>();
        services.AddScoped<ISessionWriteOnlyRepository, SessionRepository>();

        services.AddScoped<ILoggedUser, LoggedUser>();

        services.AddScoped<IPaymentWriteOnlyRepository, PaymentRepository>();
        services.AddScoped<IPaymentReadOnlyRepository, PaymentRepository>();
        services.AddScoped<IPaymentUpdateOnlyRepository, PaymentRepository>();

        services.AddScoped<ICinemaTicketWriteOnlyRepository, CinemaTicketRepository>();
        services.AddScoped<ICinemaTicketReadOnlyRepository, CinemaTicketRepository>();
    }

    private static void AddCommonInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddMigrations(services, configuration);

        services.AddScoped<IPasswordHasher, PasswordHasherArgon2id>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddAdminJwtTokenHandler(this IServiceCollection services, IConfiguration configuration)
    {
        var signingKey = configuration["Jwt:SigningKey"]!;
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var accessTokenExpirationMinutes = uint.Parse(configuration["Jwt:AccessTokenExpirationTimeMinutes"]!);
        var refreshTokenExpirationMinutes = uint.Parse(configuration["Jwt:RefreshTokenExpirationTimeMinutes"]!);

        services.AddScoped<IAdminAccessTokenGenerator>(provider =>
            new JwtTokenHandler(signingKey, issuer, audience, accessTokenExpirationMinutes, refreshTokenExpirationMinutes));

        services.AddScoped<IAdminRefreshTokenGenerator>(provider =>
            new JwtTokenHandler(signingKey, issuer, audience, accessTokenExpirationMinutes, refreshTokenExpirationMinutes));
    }

    private static void AddClientJwtTokenHandler(this IServiceCollection services, IConfiguration configuration)
    {
        var signingKey = configuration["Jwt:SigningKey"]!;
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var accessTokenExpirationMinutes = uint.Parse(configuration["Jwt:AccessTokenExpirationTimeMinutes"]!);
        var refreshTokenExpirationMinutes = uint.Parse(configuration["Jwt:RefreshTokenExpirationTimeMinutes"]!);

        services.AddScoped<IUserAccessTokenGenerator>(provider =>
            new JwtTokenHandler(signingKey, issuer, audience, accessTokenExpirationMinutes, refreshTokenExpirationMinutes));

        services.AddScoped<IUserRefreshTokenGenerator>(provider =>
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
