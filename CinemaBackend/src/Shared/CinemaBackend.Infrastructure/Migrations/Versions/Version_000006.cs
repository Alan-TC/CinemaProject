using System.Data;
using FluentMigrator;

namespace CinemaBackend.Infrastructure.Migrations.Versions;

[Migration(MigrationVersions.AlterColumnsFromTimestampToTimestampz)]
public sealed class Version_000006 : ForwardOnlyMigration
{
    public override void Up()
    {
        Execute.Sql(@"
            DO $$
            DECLARE
                r RECORD;
            BEGIN
                FOR r IN 
                    SELECT table_schema, table_name, column_name 
                    FROM information_schema.columns 
                    WHERE data_type = 'timestamp without time zone'
                        AND table_schema NOT IN ('pg_catalog', 'information_schema')
                LOOP
                    EXECUTE format('ALTER TABLE %I.%I ALTER COLUMN %I TYPE timestamptz USING %I AT TIME ZONE ''UTC''', 
                                    r.table_schema, r.table_name, r.column_name, r.column_name);
                END LOOP;
            END $$;
        ");
    }
}
