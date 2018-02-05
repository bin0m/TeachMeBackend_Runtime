namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTextColumnToExerciseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercises", "Text");
        }
    }
}
