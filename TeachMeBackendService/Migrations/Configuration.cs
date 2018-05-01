namespace TeachMeBackendService.Migrations
{
    using Microsoft.Azure.Mobile.Server.Tables;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TeachMeBackendService.Models.TeachMeBackendContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TeachMeBackendService.Models.TeachMeBackendContext";


            // Error: Cannot create more than one clustered index on table 'dbo.Patterns'. Drop the existing clustered index 'PK_dbo.Patterns' before creating another.
            // This will remove  errors when creating migrations and allow you to update the database via powershell command.
            // https://stackoverflow.com/questions/22923672/error-while-enabling-code-first-migrations-on-mobile-services-database
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());

            //For MySql future use 
            //SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
            //SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));

            
            //SetHistoryContextFactory("System.Data.SqlClient", (existingConnection, schema) => new HistoryContext(existingConnection, schema));
        }

        protected override void Seed(TeachMeBackendService.Models.TeachMeBackendContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
