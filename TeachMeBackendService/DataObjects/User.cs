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

        [Required]
        public string Password { get; set; }
        public string Salt { get; set; }

        public int CompletedСoursesCount { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(100)]
        public string AvatarPath { get; set; }

        [Required]
        public UserRole UserRole { get; set; }

        // Children Courses
        public List<Course> Courses { get; set; }

        // Children PatternStudents
        public List<PatternStudent> PatternStudents { get; set; }

        // Children Comments
        public List<Comment> Comments { get; set; }

        // Children CommentRatings
        public List<CommentRating> CommentRatings { get; set; }

    }
}