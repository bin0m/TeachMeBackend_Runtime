namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSoundColumnToExercise : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "Sound", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercises", "Sound");
        }
    }
}
