using Microsoft.Azure.Mobile.Server;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.DataObjects
{
    [Table("CourseProgresses")]
    public class CourseProgress : EntityData
     {
        public bool IsStarted { get; set; }

        public bool IsDone { get; set; }

        [NotMapped]
        public ProgressCourseModel Progress { get; set; }

        // Course FK
        public String CourseId { get; set; }

        // parent Course link
        public Course Course { get; set; }
 
         // Student FK
         public String UserId { get; set; }
 
         // parent Student link
         public User User { get; set; }
     }
}