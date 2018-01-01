namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsDoneColumnToPatternStudentsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatternStudents", "IsDone", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatternStudents", "IsDone");
        }
    }
}
