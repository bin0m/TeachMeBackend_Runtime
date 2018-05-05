using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Azure.Mobile.Server.Tables;
using TeachMeBackendService.DataObjects;

namespace TeachMeBackendService.Models
{
    //For MySql future use 
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class TeachMeBackendContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        private const string ConnectionStringName = "Name=MS_TableConnectionString";

        public TeachMeBackendContext() : base(ConnectionStringName, throwIfV1Schema: false)
        {
        }

        public static TeachMeBackendContext Create()    
        {
            return new TeachMeBackendContext();
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Section2> Section2 { get; set; }
        public DbSet<Section3> Section3 { get; set; }      
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentRating> CommentRatings { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseStudent> ExerciseStudents { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Pair> Pairs { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<User> UserDetails { get; set; }
        public DbSet<LessonProgress> LessonProgresses { get; set; }
        public DbSet<SectionProgress> SectionProgresses { get; set; }
        public DbSet<CourseProgress> CourseProgresses { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<StudyPeriod> StudyPeriods { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<PartyUser> PartyUsers { get; set; }
        public DbSet<PartyStudyProgram> PartyStudyPrograms { get; set; }
        public DbSet<StudyProgramCourse> StudyProgramCourses { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

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

            // Lessons <1-to-many> Exercises
            modelBuilder.Entity<Exercise>()
               .HasRequired(p => p.Lesson)
               .WithMany(l => l.Exercises)
               .HasForeignKey(p => p.LessonId);

            // Exercise <1-to-many> Comments
            modelBuilder.Entity<Comment>()
               .HasRequired(p => p.Exercise)
               .WithMany(l => l.Comments)
               .HasForeignKey(p => p.ExerciseId);

            // Exercise <1-to-many> Answers
            modelBuilder.Entity<Answer>()
               .HasRequired(p => p.Exercise)
               .WithMany(l => l.Answers)
               .HasForeignKey(p => p.ExerciseId);

            // Exercise <1-to-many> Pairs
            modelBuilder.Entity<Pair>()
               .HasRequired(p => p.Exercise)
               .WithMany(l => l.Pairs)
               .HasForeignKey(p => p.ExerciseId);

            // Exercise <1-to-many> Spaces
            modelBuilder.Entity<Space>()
               .HasRequired(p => p.Exercise)
               .WithMany(l => l.Spaces)
               .HasForeignKey(p => p.ExerciseId);

            // Exercise <1-to-many> ExerciseStudents
            modelBuilder.Entity<ExerciseStudent>()
               .HasRequired(p => p.Exercise)
               .WithMany(l => l.ExerciseStudents)
               .HasForeignKey(p => p.ExerciseId);

            // User <1-to-many> ExerciseStudents
            modelBuilder.Entity<ExerciseStudent>()
               .HasRequired(p => p.User)
               .WithMany(l => l.ExerciseStudents)
               .HasForeignKey(p => p.UserId);

            // Lesson <1-to-many> LessonProgresses
            modelBuilder.Entity<LessonProgress>()
                .HasRequired(p => p.Lesson)
                .WithMany(l => l.LessonProgresses)
                .HasForeignKey(p => p.LessonId);

            // User <1-to-many> LessonProgresses
            modelBuilder.Entity<LessonProgress>()
                .HasRequired(p => p.User)
                .WithMany(l => l.LessonProgresses)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(false);

            // Section <1-to-many> SectionProgresses
            modelBuilder.Entity<SectionProgress>()
                .HasRequired(p => p.Section)
                .WithMany(l => l.SectionProgresses)
                .HasForeignKey(p => p.SectionId);

            // User <1-to-many> SectionProgresses
            modelBuilder.Entity<SectionProgress>()
                .HasRequired(p => p.User)
                .WithMany(l => l.SectionProgresses)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(false);

            // Course <1-to-many> CourseProgresses
            modelBuilder.Entity<CourseProgress>()
               .HasRequired(p => p.Course)
               .WithMany(l => l.CourseProgresses)
               .HasForeignKey(p => p.CourseId);

            // User <1-to-many> CourseProgresses
            modelBuilder.Entity<CourseProgress>()
               .HasRequired(p => p.User)
               .WithMany(l => l.CourseProgresses)
               .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(false);

            // University <1-to-many> Parties
            modelBuilder.Entity<Party>()
               .HasRequired(p => p.University)
               .WithMany(l => l.Parties)
               .HasForeignKey(p => p.UniversityId);

            // Faculty <01-to-many> Parties
            modelBuilder.Entity<Party>()
               .HasOptional(c => c.Faculty)
               .WithMany(p => p.Parties)
               .HasForeignKey(c => c.FacultyId);

            // Specialty <1-to-many> Parties
            modelBuilder.Entity<Party>()
               .HasRequired(p => p.Specialty)
               .WithMany(l => l.Parties)
               .HasForeignKey(p => p.SpecialtyId);

            // StudyPeriod <1-to-many> Parties
            modelBuilder.Entity<Party>()
               .HasRequired(p => p.StudyPeriod)
               .WithMany(l => l.Parties)
               .HasForeignKey(p => p.StudyPeriodId);

            // Party <1-to-many> PartyUsers
            modelBuilder.Entity<PartyUser>()
               .HasRequired(p => p.Party)
               .WithMany(l => l.PartyUsers)
               .HasForeignKey(p => p.PartyId);

            // User <1-to-many> PartyUsers
            modelBuilder.Entity<PartyUser>()
               .HasRequired(p => p.User)
               .WithMany(l => l.PartyUsers)
               .HasForeignKey(p => p.UserId);

            // Party <1-to-many> PartyStudyPrograms
            modelBuilder.Entity<PartyStudyProgram>()
               .HasRequired(p => p.Party)
               .WithMany(l => l.PartyStudyPrograms)
               .HasForeignKey(p => p.PartyId);

            // StudyProgram <1-to-many> PartyStudyPrograms
            modelBuilder.Entity<PartyStudyProgram>()
               .HasRequired(p => p.StudyProgram)
               .WithMany(l => l.PartyStudyPrograms)
               .HasForeignKey(p => p.StudyProgramId);

            // StudyProgram <1-to-many> StudyProgramCourses
            modelBuilder.Entity<StudyProgramCourse>()
               .HasRequired(p => p.StudyProgram)
               .WithMany(l => l.StudyProgramCourses)
               .HasForeignKey(p => p.StudyProgramId);

            // Course <1-to-many> StudyProgramCourses
            modelBuilder.Entity<StudyProgramCourse>()
               .HasRequired(p => p.Course)
               .WithMany(l => l.StudyProgramCourses)
               .HasForeignKey(p => p.CourseId);
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
            var course = Courses.Include(c => c.Sections).Include(c => c.CourseProgresses).FirstOrDefault(c => c.Id == id);

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

                // delete inner CourseProgresses
                CourseProgresses.RemoveRange(course.CourseProgresses);

                Courses.Remove(course);
                return course;
            }

            return null;
        }

    }

}
