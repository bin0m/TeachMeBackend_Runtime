using System;
using Microsoft.Azure.Mobile.Server;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace TeachMeBackendService.DataObjects
{
    public enum UserRole {
        Student = 0,
        Teacher = 1,
        Admin= 2
    }

    public class User : EntityData
    {
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        [Required]
        public string Login { get; set; }

        public int CompletedCoursesCount { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(100)]
        public string AvatarPath { get; set; }

        [Required]
        public UserRole UserRole { get; set; }

        public string FacebookId { get; set; }

        // Children Courses
        public List<Course> Courses { get; set; }

        // Children SectionProgresses
        public List<SectionProgress> SectionProgresses { get; set; }

        // Children LessonProgresses
        public List<LessonProgress> LessonProgresses { get; set; }

        // Children ExerciseStudents
        public List<ExerciseStudent> ExerciseStudents { get; set; }

        // Children Comments
        public List<Comment> Comments { get; set; }

        // Children CommentRatings
        public List<CommentRating> CommentRatings { get; set; }

        // Children CourseProgresses
        public List<CourseProgress> CourseProgresses { get; set; }

        // Children GroupUsers
        public List<GroupUser> GroupUsers { get; set; }

    }
}