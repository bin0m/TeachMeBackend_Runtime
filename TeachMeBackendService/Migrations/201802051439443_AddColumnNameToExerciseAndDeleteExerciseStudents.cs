namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnNameToExerciseAndDeleteExerciseStudents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExerciseStudents", "ExerciseId", "dbo.Exercises");
            DropForeignKey("dbo.ExerciseStudents", "UserId", "dbo.Users");
            DropIndex("dbo.ExerciseStudents", new[] { "ExerciseId" });
            DropIndex("dbo.ExerciseStudents", new[] { "UserId" });
            AddColumn("dbo.Exercises", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ExerciseStudents", "ExerciseId", c => c.String(maxLength: 128));
            AlterColumn("dbo.ExerciseStudents", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ExerciseStudents", "ExerciseId");
            CreateIndex("dbo.ExerciseStudents", "UserId");
            AddForeignKey("dbo.ExerciseStudents", "ExerciseId", "dbo.Exercises", "Id");
            AddForeignKey("dbo.ExerciseStudents", "UserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExerciseStudents", "UserId", "dbo.Users");
            DropForeignKey("dbo.ExerciseStudents", "ExerciseId", "dbo.Exercises");
            DropIndex("dbo.ExerciseStudents", new[] { "UserId" });
            DropIndex("dbo.ExerciseStudents", new[] { "ExerciseId" });
            AlterColumn("dbo.ExerciseStudents", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ExerciseStudents", "ExerciseId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Exercises", "Name");
            CreateIndex("dbo.ExerciseStudents", "UserId");
            CreateIndex("dbo.ExerciseStudents", "ExerciseId");
            AddForeignKey("dbo.ExerciseStudents", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExerciseStudents", "ExerciseId", "dbo.Exercises", "Id", cascadeDelete: true);
        }
    }
}
