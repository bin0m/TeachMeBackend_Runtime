namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablesExercisesExerciseStudentsAnswersPairsDeletePatternsTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PatternStudents", "PatternId", "dbo.Patterns");
            DropForeignKey("dbo.Patterns", "LessonId", "dbo.Lessons");
            RenameTable(name: "dbo.PatternStudents", newName: "ExerciseStudents");           
            DropForeignKey("dbo.Comments", "PatternId", "dbo.Patterns");
            DropIndex("dbo.Comments", new[] { "PatternId" });
            DropIndex("dbo.Patterns", new[] { "LessonId" });
            DropIndex("dbo.Patterns", new[] { "CreatedAt" });
            DropIndex("dbo.ExerciseStudents", new[] { "PatternId" });
            CreateTable(
                "dbo.Answers",
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
                        Title = c.String(),
                        IsRight = c.Boolean(nullable: false),
                        ExerciseId = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exercises", t => t.ExerciseId, cascadeDelete: true)
                .Index(t => t.ExerciseId)
                .Index(t => t.CreatedAt, clustered: true);
            
            CreateTable(
                "dbo.Exercises",
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
                        Type = c.String(nullable: false),
                        Question = c.String(),
                        Answer = c.String(),
                        Image = c.String(),
                        LessonId = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .Index(t => t.LessonId)
                .Index(t => t.CreatedAt, clustered: true);
            
            CreateTable(
                "dbo.Pairs",
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
                        Value = c.String(),
                        Equal = c.String(),
                        ExerciseId = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exercises", t => t.ExerciseId, cascadeDelete: true)
                .Index(t => t.ExerciseId)
                .Index(t => t.CreatedAt, clustered: true);
            
            AddColumn("dbo.Comments", "ExerciseId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.ExerciseStudents", "ExerciseId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Comments", "ExerciseId");
            CreateIndex("dbo.ExerciseStudents", "ExerciseId");
            AddForeignKey("dbo.ExerciseStudents", "ExerciseId", "dbo.Exercises", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "ExerciseId", "dbo.Exercises", "Id", cascadeDelete: true);
            DropColumn("dbo.Comments", "PatternId");
            DropColumn("dbo.ExerciseStudents", "PatternId");
            DropTable("dbo.Patterns",
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
                "dbo.Patterns",
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
                        Name = c.String(nullable: false),
                        JsonText = c.String(nullable: false),
                        Type = c.String(),
                        LessonId = c.String(nullable: false, maxLength: 128),
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
            
            AddColumn("dbo.ExerciseStudents", "PatternId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Comments", "PatternId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Answers", "ExerciseId", "dbo.Exercises");
            DropForeignKey("dbo.Pairs", "ExerciseId", "dbo.Exercises");
            DropForeignKey("dbo.Exercises", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.Comments", "ExerciseId", "dbo.Exercises");
            DropForeignKey("dbo.ExerciseStudents", "ExerciseId", "dbo.Exercises");
            DropIndex("dbo.Pairs", new[] { "CreatedAt" });
            DropIndex("dbo.Pairs", new[] { "ExerciseId" });
            DropIndex("dbo.ExerciseStudents", new[] { "ExerciseId" });
            DropIndex("dbo.Comments", new[] { "ExerciseId" });
            DropIndex("dbo.Exercises", new[] { "CreatedAt" });
            DropIndex("dbo.Exercises", new[] { "LessonId" });
            DropIndex("dbo.Answers", new[] { "CreatedAt" });
            DropIndex("dbo.Answers", new[] { "ExerciseId" });
            DropColumn("dbo.ExerciseStudents", "ExerciseId");
            DropColumn("dbo.Comments", "ExerciseId");
            DropTable("dbo.Pairs",
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
            DropTable("dbo.Exercises",
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
            DropTable("dbo.Answers",
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
            CreateIndex("dbo.ExerciseStudents", "PatternId");
            CreateIndex("dbo.Patterns", "CreatedAt", clustered: true);
            CreateIndex("dbo.Patterns", "LessonId");
            CreateIndex("dbo.Comments", "PatternId");
            AddForeignKey("dbo.Comments", "PatternId", "dbo.Patterns", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Patterns", "LessonId", "dbo.Lessons", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PatternStudents", "PatternId", "dbo.Patterns", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.ExerciseStudents", newName: "PatternStudents");
        }
    }
}
