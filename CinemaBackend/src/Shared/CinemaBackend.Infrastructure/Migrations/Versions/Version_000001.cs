using FluentMigrator;

namespace CinemaBackend.Infrastructure.Migrations.Versions;


[Migration(MigrationVersions.AdminUserTable)]
public sealed class Version_000001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("admin_users")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("deactivated_at").AsDateTime().Nullable()
            .WithColumn("email").AsString(256).Unique().NotNullable()
            .WithColumn("password_hash").AsString(256).NotNullable();
    }
}
