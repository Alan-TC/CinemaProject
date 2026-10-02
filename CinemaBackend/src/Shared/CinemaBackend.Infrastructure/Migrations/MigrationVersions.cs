namespace CinemaBackend.Infrastructure.Migrations;

internal abstract class MigrationVersions
{
    public const uint AdminUserTable = 1;
    public const uint MovieRelatedTablesAndImageTable = 2;
    public const uint TheaterLayoutRelatedTables = 3;
    public const uint SessionRelatedTables = 4;
    public const uint FixUniqueConstraintSessionRelatedTables = 5;
    public const uint AlterColumnsFromTimestampToTimestampz = 6;
}
