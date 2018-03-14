namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameColumnToCourseProgress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseProgresses", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourseProgresses", "Name");
        }
    }
}
