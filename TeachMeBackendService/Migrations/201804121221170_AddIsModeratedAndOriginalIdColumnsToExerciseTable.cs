namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsModeratedAndOriginalIdColumnsToExerciseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "isModerated", c => c.Boolean(nullable: false));
            AddColumn("dbo.Exercises", "OriginalId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercises", "OriginalId");
            DropColumn("dbo.Exercises", "isModerated");
        }
    }
}
