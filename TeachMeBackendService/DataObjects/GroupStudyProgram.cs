using Microsoft.Azure.Mobile.Server;
using System;


namespace TeachMeBackendService.DataObjects
{
    public class GroupStudyProgram : EntityData
    {
        // parent Group FK
        public String GroupId { get; set; }

        // parent Group link
        public Group Group { get; set; }

        // parent StudyProgram FK
        public String StudyProgramId { get; set; }

        // parent StudyProgram link
        public StudyProgram StudyProgram { get; set; }
    }
}