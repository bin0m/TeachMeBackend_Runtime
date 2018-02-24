namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePasswordColumnFromOldUserTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Password");
            DropColumn("dbo.Users", "Salt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Salt", c => c.String());
            AddColumn("dbo.Users", "Password", c => c.String(nullable: false));
        }
    }
}
