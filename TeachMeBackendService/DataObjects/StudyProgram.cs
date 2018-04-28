using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class StudyProgram : EntityData
    {
        [StringLength(128)]
        public string Name { get; set; }

        // Children GroupStudyPrograms
        public List<GroupStudyProgram> GroupStudyPrograms { get; set; }

        // Children StudyProgramCourses
        public List<StudyProgramCourse> StudyProgramCourses { get; set; }
    }
}