using Microsoft.Azure.Mobile.Server;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.DataObjects
{
    [Table("LessonProgresses")]
    public class LessonProgress : EntityData
     {
        public bool IsStarted { get; set; }

        public bool IsDone { get; set; }

         [NotMapped]
         public ProgressLessonModel Progress { get; set; }

        // Lesson FK
        public String LessonId { get; set; }
 
         // parent Lesson link
         public Lesson Lesson { get; set; }
 
         // Student FK
         public String UserId { get; set; }
 
         // parent Student link
         public User User { get; set; }
     }
}