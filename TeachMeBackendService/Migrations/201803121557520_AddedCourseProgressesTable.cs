namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCourseProgressesTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentCourses", "UserId", "dbo.Users");
            DropIndex("dbo.StudentCourses", new[] { "CourseId" });
            DropIndex("dbo.StudentCourses", new[] { "UserId" });
            CreateTable(
                "dbo.CourseProgresses",
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
                        IsStarted = c.Boolean(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        CourseId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
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
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.CourseId)
                .Index(t => t.UserId)
                .Index(t => t.CreatedAt, clustered: true);
            
            AlterColumn("dbo.StudentCourses", "CourseId", c => c.String(maxLength: 128));
            AlterColumn("dbo.StudentCourses", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.StudentCourses", "CourseId");
            CreateIndex("dbo.StudentCourses", "UserId");
            AddForeignKey("dbo.StudentCourses", "UserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentCourses", "UserId", "dbo.Users");
            DropForeignKey("dbo.CourseProgresses", "UserId", "dbo.Users");
            DropForeignKey("dbo.CourseProgresses", "CourseId", "dbo.Courses");
            DropIndex("dbo.StudentCourses", new[] { "UserId" });
            DropIndex("dbo.StudentCourses", new[] { "CourseId" });
            DropIndex("dbo.CourseProgresses", new[] { "CreatedAt" });
            DropIndex("dbo.CourseProgresses", new[] { "UserId" });
            DropIndex("dbo.CourseProgresses", new[] { "CourseId" });
            AlterColumn("dbo.StudentCourses", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StudentCourses", "CourseId", c => c.String(nullable: false, maxLength: 128));
            DropTable("dbo.CourseProgresses",
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
            CreateIndex("dbo.StudentCourses", "UserId");
            CreateIndex("dbo.StudentCourses", "CourseId");
            AddForeignKey("dbo.StudentCourses", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
