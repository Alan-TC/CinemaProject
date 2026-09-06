using System.Data;
using FluentMigrator;

namespace CinemaBackend.Infrastructure.Migrations.Versions;

[Migration(MigrationVersions.MovieRelatedTablesAndImageTable)]
public sealed class Version_000002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("movies")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("deactivated_at").AsDateTime().Nullable()
            .WithColumn("title").AsString(256).NotNullable()
            .WithColumn("sinopse").AsString(2048).NotNullable()
            .WithColumn("duration").AsTime().NotNullable()
            .WithColumn("release_date").AsDateTime().NotNullable()
            .WithColumn("class_ind").AsString(64).NotNullable();
            
        Create.Table("film_genres")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("deactivated_at").AsDateTime().Nullable()
            .WithColumn("genre").AsString(64).NotNullable()
            .WithColumn("movie_id").AsGuid().NotNullable()
                .ForeignKey("FK_film_genres_movies", "movies", "id").OnDelete(Rule.Cascade);
    }
}
