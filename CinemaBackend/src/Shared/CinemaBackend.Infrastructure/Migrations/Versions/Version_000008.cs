using FluentMigrator;
using System.Data;

namespace CinemaBackend.Infrastructure.Migrations.Versions;


[Migration(MigrationVersions.PaymentTable)]
public sealed class Version_000008 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("payments")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("deactivated_at").AsDateTimeOffset().Nullable()
            .WithColumn("full_price_quantity").AsInt32().NotNullable()
            .WithColumn("half_price_quantity").AsInt32().NotNullable()
            .WithColumn("total").AsDecimal(19, 4).NotNullable()
            .WithColumn("status").AsString(64).NotNullable()
            .WithColumn("user_id").AsGuid().NotNullable()
                .ForeignKey("FK_payments_users", "users", "id").OnDelete(Rule.None)
            .WithColumn("session_id").AsGuid().NotNullable()
                .ForeignKey("FK_payments_sessions", "sessions", "id").OnDelete(Rule.None);

        Create.Table("allocated_seats")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("seat_code").AsString(64).NotNullable()
            .WithColumn("seat_id").AsGuid().NotNullable()
            .WithColumn("session_id").AsGuid().NotNullable()
            .WithColumn("payment_id").AsGuid().NotNullable()
                .ForeignKey("FK_allocated_seats_payments", "payments", "id").OnDelete(Rule.Cascade);
    }
}
