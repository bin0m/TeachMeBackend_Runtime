using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using TeachMeBackendService.DataObjects;

namespace TeachMeBackendService.Models
{
    //For MySql future use 
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class TeachMeBackendContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        private const string connectionStringName = "Name=MS_TableConnectionString";

        public TeachMeBackendContext() : base(connectionStringName)
        {
        } 

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Section2> Section2s { get; set; }
        public DbSet<Section3> Section3s { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<Course> Courses { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            // Course <one-to-many> Sections
            modelBuilder.Entity<Section>()
               .HasRequired(c => c.Course)
               .WithMany(p => p.Sections)
               .HasForeignKey(c => c.CourseId);

            // Section <one-to-many> Section2s
            modelBuilder.Entity<Section2>()
               .HasRequired(c => c.Section)
               .WithMany(p => p.Section2s)
               .HasForeignKey(c => c.SectionId);

            // Section2 <one-to-many> Section3s
            modelBuilder.Entity<Section3>()
               .HasRequired(c => c.Section2)
               .WithMany(p => p.Section3s)
               .HasForeignKey(c => c.Section2Id);

            // Section <01-to-many> Lessons
            modelBuilder.Entity<Lesson>()
               .HasOptional(c => c.Section)
               .WithMany(p => p.Lessons)
               .HasForeignKey(c => c.SectionId);

            // Section2 <01-to-many> Lessons
            modelBuilder.Entity<Lesson>()
               .HasOptional(c => c.Section3)
               .WithMany(p => p.Lessons)
               .HasForeignKey(c => c.Section2Id);

            // Section3 <01-to-many> Lessons
            modelBuilder.Entity<Lesson>()
               .HasOptional(c => c.Section3)
               .WithMany(p => p.Lessons)
               .HasForeignKey(c => c.Section3Id);

            // Lessons <many-to-many> Patterns
            modelBuilder.Entity<Lesson>()
                .HasMany(l => l.Patterns)
                .WithMany(p => p.Lessons)
                .Map(m =>
                {
                    // Link to intermediate table
                    m.ToTable("LessonPatterns");

                    // Set up foreign keys in intermediate table
                    m.MapLeftKey("LessonId");
                    m.MapRightKey("PatternId");
                })
                ;

        }

        
    }

}
