namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateOfBirthAndAvatarFieldsToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Users", "AvatarPath", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AvatarPath");
            DropColumn("dbo.Users", "DateOfBirth");
        }
    }
}
