namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedSyntaxErrorInFieldCompletedCoursesCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CompletedCoursesCount", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "CompletedСoursesCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CompletedСoursesCount", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "CompletedCoursesCount");
        }
    }
}
