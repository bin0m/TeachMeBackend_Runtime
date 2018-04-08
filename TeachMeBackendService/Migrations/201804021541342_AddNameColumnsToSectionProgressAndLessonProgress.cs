namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameColumnsToSectionProgressAndLessonProgress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LessonProgresses", "Name", c => c.String());
            AddColumn("dbo.SectionProgresses", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SectionProgresses", "Name");
            DropColumn("dbo.LessonProgresses", "Name");
        }
    }
}
