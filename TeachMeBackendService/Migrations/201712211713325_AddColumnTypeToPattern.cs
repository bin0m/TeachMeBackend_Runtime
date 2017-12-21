namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnTypeToPattern : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patterns", "Type", c => c.String());
            AlterColumn("dbo.Patterns", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Patterns", "JsonText", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patterns", "JsonText", c => c.String());
            AlterColumn("dbo.Patterns", "Name", c => c.String());
            DropColumn("dbo.Patterns", "Type");
        }
    }
}
