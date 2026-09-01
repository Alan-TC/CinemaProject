using CinemaBackend.Infrastructure.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBackend.Infrastructure;

public static class ServiceProviderExtensions
{
    public static async Task ApplyMigrations(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();

        DatabaseMigration.ExecuteMigrations(scope.ServiceProvider);
    }
}
