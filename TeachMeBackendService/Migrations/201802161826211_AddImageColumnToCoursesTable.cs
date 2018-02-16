namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageColumnToCoursesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Image");
        }
    }
}
