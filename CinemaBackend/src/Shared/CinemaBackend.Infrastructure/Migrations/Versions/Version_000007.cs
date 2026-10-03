using FluentMigrator;

namespace CinemaBackend.Infrastructure.Migrations.Versions;


[Migration(MigrationVersions.UserTable)]
public sealed class Version_000007 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("deactivated_at").AsDateTimeOffset().Nullable()
            .WithColumn("name").AsString(256).Unique().NotNullable()
            .WithColumn("email").AsString(256).Unique().NotNullable()
            .WithColumn("password_hash").AsString(256).NotNullable();
    }
}
