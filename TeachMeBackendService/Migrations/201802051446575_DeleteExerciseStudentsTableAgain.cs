namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteExerciseStudentsTableAgain : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExerciseStudents", "ExerciseId", "dbo.Exercises");
            DropForeignKey("dbo.ExerciseStudents", "UserId", "dbo.Users");
            DropIndex("dbo.ExerciseStudents", new[] { "ExerciseId" });
            DropIndex("dbo.ExerciseStudents", new[] { "UserId" });
            DropIndex("dbo.ExerciseStudents", new[] { "CreatedAt" });
            DropTable("dbo.ExerciseStudents",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ExerciseStudents",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        IsDone = c.Boolean(nullable: false),
                        StudentAnswer = c.String(),
                        ExerciseId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ExerciseStudents", "CreatedAt", clustered: true);
            CreateIndex("dbo.ExerciseStudents", "UserId");
            CreateIndex("dbo.ExerciseStudents", "ExerciseId");
            AddForeignKey("dbo.ExerciseStudents", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.ExerciseStudents", "ExerciseId", "dbo.Exercises", "Id");
        }
    }
}
