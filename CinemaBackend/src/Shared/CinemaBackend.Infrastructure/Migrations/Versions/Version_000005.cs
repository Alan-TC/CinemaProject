using FluentMigrator;

namespace CinemaBackend.Infrastructure.Migrations.Versions;

[Migration(MigrationVersions.FixUniqueConstraintSessionRelatedTables)]
public sealed class Version_000005 : ForwardOnlyMigration
{
    public override void Up()
    {
        Delete.Index("IX_session_rooms_tag").OnTable("session_rooms");
        
        Delete.Index("IX_session_room_seats_code").OnTable("session_room_seats");

        Create.UniqueConstraint("UQ_session_rooms_tag_session")
            .OnTable("session_rooms")
            .Columns("tag", "session_id");
        
        Create.UniqueConstraint("UQ_session_rooms_seats_code_session_room")
            .OnTable("session_room_seats")
            .Columns("code", "session_room_id");
    }
}
