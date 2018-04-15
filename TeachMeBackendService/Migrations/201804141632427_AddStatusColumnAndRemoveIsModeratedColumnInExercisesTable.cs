namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusColumnAndRemoveIsModeratedColumnInExercisesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Exercises", "isModerated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exercises", "isModerated", c => c.Boolean(nullable: false));
            DropColumn("dbo.Exercises", "Status");
        }
    }
}
