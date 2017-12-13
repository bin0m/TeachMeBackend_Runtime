namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ALterPatternsToLessonRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LessonPatterns", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.LessonPatterns", "PatternId", "dbo.Patterns");
            DropIndex("dbo.LessonPatterns", new[] { "LessonId" });
            DropIndex("dbo.LessonPatterns", new[] { "PatternId" });
            AddColumn("dbo.Patterns", "LessonId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Patterns", "LessonId");
            AddForeignKey("dbo.Patterns", "LessonId", "dbo.Lessons", "Id", cascadeDelete: true);
            DropTable("dbo.LessonPatterns");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LessonPatterns",
                c => new
                    {
                        LessonId = c.String(nullable: false, maxLength: 128),
                        PatternId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LessonId, t.PatternId });
            
            DropForeignKey("dbo.Patterns", "LessonId", "dbo.Lessons");
            DropIndex("dbo.Patterns", new[] { "LessonId" });
            DropColumn("dbo.Patterns", "LessonId");
            CreateIndex("dbo.LessonPatterns", "PatternId");
            CreateIndex("dbo.LessonPatterns", "LessonId");
            AddForeignKey("dbo.LessonPatterns", "PatternId", "dbo.Patterns", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LessonPatterns", "LessonId", "dbo.Lessons", "Id", cascadeDelete: true);
        }
    }
}
