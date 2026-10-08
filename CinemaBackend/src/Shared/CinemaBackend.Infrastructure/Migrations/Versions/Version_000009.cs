using FluentMigrator;
using System.Data;

namespace CinemaBackend.Infrastructure.Migrations.Versions;


[Migration(MigrationVersions.CinemaTicketsTable)]
public sealed class Version_000009 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("cinema_tickets")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("deactivated_at").AsDateTimeOffset().Nullable()
            .WithColumn("code").AsString(8).NotNullable()
            .WithColumn("payment_id").AsGuid().NotNullable()
                .ForeignKey("FK_cinema_tickets_payments", "payments", "id").OnDelete(Rule.None);
    }
}
