namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFacebookIdColumnToUsersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FacebookId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "FacebookId");
        }
    }
}
