using System;
using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.DataObjects
{
    public enum ModerationStatus
    {
        Draft = 0,
        FirstModeration = 1,
        SecondModeration = 2,
        Published = 3,
        Rejected = 4
    }

    public class Course : EntityData
    {
        public string Name { get; set; }
       
        public int Days { get; set; }

        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public ProgressCourseModel Progress { get; set; }

        public ModerationStatus Status { get; set; }

        // parent User FK
        public String UserId { get; set; }

        // parent User link
        public User User { get; set; }

        // Children Sections
        public List<Section> Sections { get; set; }

        // Children CourseProgresses
        public List<CourseProgress> CourseProgresses { get; set; }
    }
}