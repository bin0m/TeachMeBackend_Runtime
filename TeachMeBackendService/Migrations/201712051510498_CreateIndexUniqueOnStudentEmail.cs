namespace TeachMeBackendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateIndexUniqueOnStudentEmail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "Email", c => c.String(maxLength: 100));
            CreateIndex("dbo.Students", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Students", new[] { "Email" });
            AlterColumn("dbo.Students", "Email", c => c.String());
        }
    }
}
