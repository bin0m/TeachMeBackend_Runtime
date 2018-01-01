using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeachMeBackendService.DataObjects
{
    public class Pattern : EntityData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string JsonText { get; set; }

        public string Type { get; set; }

        // parent Lesson FK
        public String LessonId { get; set; }

        // parent Lesson link
        public Lesson Lesson { get; set; }

        // Children PatternStudents
        public List<PatternStudent> PatternStudents { get; set; }
    }
}