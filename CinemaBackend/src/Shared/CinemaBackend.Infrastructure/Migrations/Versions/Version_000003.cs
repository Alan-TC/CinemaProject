using System.Data;
using FluentMigrator;

namespace CinemaBackend.Infrastructure.Migrations.Versions;

[Migration(MigrationVersions.TheaterLayoutRelatedTables)]
public sealed class Version_000003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("theater_layouts")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("deactivated_at").AsDateTime().Nullable()
            .WithColumn("tag").AsString(8).NotNullable().Unique();
        
        Create.Table("layout_grids")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("width").AsInt32().NotNullable()
            .WithColumn("height").AsInt32().NotNullable()
            .WithColumn("grid").AsCustom("jsonb").NotNullable()
            .WithColumn("theater_layout_id").AsGuid().NotNullable()
                .ForeignKey("FK_layout_grid_theater_layouts", "theater_layouts", "id")
                .OnDelete(Rule.Cascade);
    }
}
