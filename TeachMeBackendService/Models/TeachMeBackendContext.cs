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
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Section2> Section2 { get; set; }
        public DbSet<Section3> Section3 { get; set; }      
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<PatternStudent> PatternStudents { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentRating> CommentRatings { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            // Teachers <one-to-many> Courses
            modelBuilder.Entity<Course>()
               .HasRequired(c => c.User)
               .WithMany(t => t.Courses)
               .HasForeignKey(c => c.UserId);

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

            // Lessons <1-to-many> Patterns
            modelBuilder.Entity<Pattern>()
               .HasRequired(p => p.Lesson)
               .WithMany(l => l.Patterns)
               .HasForeignKey(p => p.LessonId);

            // Pattern <1-to-many> PatternStudents
            modelBuilder.Entity<PatternStudent>()
               .HasRequired(p => p.Pattern)
               .WithMany(l => l.PatternStudents)
               .HasForeignKey(p => p.PatternId);

            // User <1-to-many> PatternStudents
            modelBuilder.Entity<PatternStudent>()
               .HasRequired(p => p.User)
               .WithMany(l => l.PatternStudents)
               .HasForeignKey(p => p.UserId);

            // Pattern <1-to-many> Comments
            modelBuilder.Entity<Comment>()
               .HasRequired(p => p.Pattern)
               .WithMany(l => l.Comments)
               .HasForeignKey(p => p.PatternId);

            // User <1-to-many> Comments
            modelBuilder.Entity<Comment>()
               .HasRequired(p => p.User)
               .WithMany(l => l.Comments)
               .HasForeignKey(p => p.UserId);

            // Comment <1-to-many> CommentRatings
            modelBuilder.Entity<CommentRating>()
               .HasRequired(cr => cr.Comment)
               .WithMany(l => l.CommentRatings)
               .HasForeignKey(cr => cr.CommentId);

            // User <1-to-many> CommentRatings
            modelBuilder.Entity<CommentRating>()
               .HasRequired(cr => cr.User)
               .WithMany(l => l.CommentRatings)
               .HasForeignKey(cr => cr.UserId)
               .WillCascadeOnDelete(false);


        }

        /// <summary>
        /// Delete Section And ALL Its Children
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1)section, when deletes all succesfully  2) null, when failed to delete section or one of its children</returns>
        public Section DeleteSectionAndChildren(string id)
        {
            var section = Sections.Include(c => c.Lessons).FirstOrDefault(c => c.Id == id);

            if (section != null)
            {
                Lessons.RemoveRange(section.Lessons);
                Sections.Remove(section);
                return section;
            }            

            return null;
        }


        /// <summary>
        /// Delete Course And ALL Its Children
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1)course, when deletes all succesfully  2) null, when failed to delete course or one of its children</returns>
        public Course DeleteCourseAndChildren(string id)
        {
            var course = Courses.Include(c => c.Sections).FirstOrDefault(c => c.Id == id);

            if (course != null)
            {
                var ids = course.Sections.Select(s => s.Id).ToList();
                foreach (var sectionId in ids)
                {
                    var result = DeleteSectionAndChildren(sectionId);
                    if (result == null)
                    {
                        return null;
                    }
                }
                Courses.Remove(course);
                return course;
            }

            return null;
        }

    }

}
