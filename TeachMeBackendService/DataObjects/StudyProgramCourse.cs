using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class StudyProgramCourse : EntityData
    {
           // parent StudyProgram FK
        public String StudyProgramId { get; set; }

        // parent StudyProgram link
        public StudyProgram StudyProgram { get; set; }

        // parent Course FK
        public String CourseId { get; set; }

        // parent Course link
        public Course Course { get; set; }
    }
}