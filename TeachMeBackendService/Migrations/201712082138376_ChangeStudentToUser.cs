namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStudentToUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Section2", newName: "Section2s");
            RenameTable(name: "dbo.Section3", newName: "Section3s");
            RenameTable(name: "dbo.Students", newName: "Users");
            AddColumn("dbo.Users", "Salt", c => c.String());
            AddColumn("dbo.Users", "UserRole", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "FullName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String());
            AlterColumn("dbo.Users", "Login", c => c.String());
            AlterColumn("dbo.Users", "FullName", c => c.String());
            DropColumn("dbo.Users", "UserRole");
            DropColumn("dbo.Users", "Salt");
            RenameTable(name: "dbo.Users", newName: "Students");
            RenameTable(name: "dbo.Section3s", newName: "Section3");
            RenameTable(name: "dbo.Section2s", newName: "Section2");
        }
    }
}
