using System;
using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace TeachMeBackendService.DataObjects
{
    public class Course : EntityData
    {
        public string Name { get; set; }
        public int Days { get; set; }

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