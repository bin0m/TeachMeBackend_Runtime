
namespace TeachMeBackendService.Models
{
    public class ProgressLessonModel
    {
        public bool IsStarted { get; set; }
        public bool IsDone { get; set; }
        public int ExercisesNumber { get; set; }
        public int ExercisesDone { get; set; }
    }

    public class ProgressSectionModel
    {
        public bool IsStarted { get; set; }
        public bool IsDone { get; set; }
        public int LessonsNumber { get; set; }
        public int LessonsDone { get; set; }
    }

    public class ProgressCourseModel
    {
        public bool IsStarted { get; set; }
        public bool IsDone { get; set; }
        public int SectionsNumber { get; set; }
        public int SectionsDone { get; set; }
    }
}