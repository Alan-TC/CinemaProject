using System.Data;
using FluentMigrator;

namespace CinemaBackend.Infrastructure.Migrations.Versions;

[Migration(MigrationVersions.SessionRelatedTables)]
public sealed class Version_000004 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("sessions")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("deactivated_at").AsDateTime().Nullable()
            .WithColumn("code").AsString(8).NotNullable().Unique()
            .WithColumn("starts_at").AsDateTime().NotNullable().NotNullable()
            .WithColumn("ends_at").AsDateTime().NotNullable().NotNullable()
            .WithColumn("start_ticket_sales_at").AsDateTime().NotNullable().NotNullable()
            .WithColumn("movie_id").AsGuid().NotNullable().NotNullable()
                .ForeignKey("FK_session_movies", "movies", "id")
                .OnDelete(Rule.None);
        
        Create.Table("session_pricings")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("normal_price").AsDecimal(19, 4).NotNullable()
            .WithColumn("session_id").AsGuid().NotNullable()
                .ForeignKey("FK_session_pricing_sessions", "sessions", "id")
                .OnDelete(Rule.Cascade);
        
        Create.Table("session_rooms")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("tag").AsString(8).Unique().NotNullable()
            .WithColumn("width").AsInt32().NotNullable()
            .WithColumn("height").AsInt32().NotNullable()
            .WithColumn("session_id").AsGuid().NotNullable()
                .ForeignKey("FK_session_room_sessions", "sessions", "id")
                .OnDelete(Rule.Cascade);
        
        Create.Table("session_room_seats")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("code").AsString(8).Unique().NotNullable()
            .WithColumn("pos_x").AsInt32().NotNullable()
            .WithColumn("pos_y").AsInt32().NotNullable()
            .WithColumn("status").AsString(64).NotNullable()
            .WithColumn("session_room_id").AsGuid().NotNullable()
                .ForeignKey("FK_session_room_seat_session_rooms", "session_rooms", "id")
                .OnDelete(Rule.Cascade);

        Create.UniqueConstraint("UQ_session_room_seats_session_posxy")
            .OnTable("session_room_seats")
            .Columns("session_room_id", "pos_x", "pos_y");
    }
}
