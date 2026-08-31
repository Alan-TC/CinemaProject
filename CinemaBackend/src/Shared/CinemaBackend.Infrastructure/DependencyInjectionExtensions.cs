using System.Reflection;
using CinemaBackend.Infrastructure.Data;
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
        AddDatabase(services, configuration);
        AddMigrations(services, configuration);
    }

    public static void AddClientInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddMigrations(services, configuration);
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
