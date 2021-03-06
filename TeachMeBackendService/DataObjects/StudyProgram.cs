﻿using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TeachMeBackendService.DataObjects
{
    public class StudyProgram : EntityData
    {
        [StringLength(128)]
        public string Name { get; set; }

        // Children PartyStudyPrograms
        public List<PartyStudyProgram> PartyStudyPrograms { get; set; }

        // Children StudyProgramCourses
        public List<StudyProgramCourse> StudyProgramCourses { get; set; }
    }
}