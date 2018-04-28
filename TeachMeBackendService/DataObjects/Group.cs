using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class Group : EntityData
    {
        [StringLength(128)]
        public string Name { get; set; }

        public int YearOfStudy { get; set; }

        // parent University FK
        public String UniversityId { get; set; }

        // parent University link
        public University University { get; set; }

        // parent StudyPeriod FK
        public String StudyPeriodId { get; set; }

        // parent StudyPeriod link
        public StudyPeriod StudyPeriod { get; set; }

        // parent Faculty FK
        public String FacultyId { get; set; }

        // parent Faculty link
        public Faculty Faculty { get; set; }

        // parent Specialty FK
        public String SpecialtyId { get; set; }

        // parent Specialty link
        public Specialty Specialty { get; set; }

        // Children GroupUsers
        public List<GroupUser> GroupUsers { get; set; }

        // Children GroupStudyPrograms
        public List<GroupStudyProgram> GroupStudyPrograms { get; set; }
    }
}