using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBackend.Infrastructure.Migrations;

internal sealed class DatabaseMigration
{
    public static void ExecuteMigrations(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();

        runner.MigrateUp();
    }
}
