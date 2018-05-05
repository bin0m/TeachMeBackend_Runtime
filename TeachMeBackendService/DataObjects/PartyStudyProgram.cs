using Microsoft.Azure.Mobile.Server;
using System;


namespace TeachMeBackendService.DataObjects
{
    public class PartyStudyProgram : EntityData
    {
        // parent Party FK
        public String PartyId { get; set; }

        // parent Party link
        public Party Party { get; set; }

        // parent StudyProgram FK
        public String StudyProgramId { get; set; }

        // parent StudyProgram link
        public StudyProgram StudyProgram { get; set; }
    }
}