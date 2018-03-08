using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.DataObjects
{
    public class Section : EntityData
    {
        public string Name { get; set; }

        [NotMapped]
        public ProgressModel Progress { get; set; }

        // parent Course FK
        public String CourseId { get; set; }

        // parent Course link
        public Course Course { get; set; }

        //children Section2s
        public List<Section2> Section2s { get; set; }
    
        //children Lessons
        public List<Lesson> Lessons { get; set; }
    }
}