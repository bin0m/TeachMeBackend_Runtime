namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionAndKeywordsColumnsToCoursesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Description", c => c.String());
            AddColumn("dbo.Courses", "Keywords", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Keywords");
            DropColumn("dbo.Courses", "Description");
        }
    }
}
