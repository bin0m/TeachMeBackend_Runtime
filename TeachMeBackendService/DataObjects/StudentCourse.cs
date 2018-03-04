using Microsoft.Azure.Mobile.Server;
using System;

namespace TeachMeBackendService.DataObjects
{
    public class StudentCourse : EntityData
     {
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