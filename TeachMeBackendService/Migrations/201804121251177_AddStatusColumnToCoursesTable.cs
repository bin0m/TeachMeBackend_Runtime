namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusColumnToCoursesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Status");
        }
    }
}
