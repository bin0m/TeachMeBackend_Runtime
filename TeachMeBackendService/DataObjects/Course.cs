using System;
using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.DataObjects
{
    public class Course : EntityData
    {
        public string Name { get; set; }
       
        public int Days { get; set; }

        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public ProgressModel Progress { get; set; }

        // parent User FK
        public String UserId { get; set; }

        // parent User link
        public User User { get; set; }

        // Children Sections
        public List<Section> Sections { get; set; }

        // Children StudentCourses
        public List<StudentCourse> StudentCourses { get; set; }
    }
}