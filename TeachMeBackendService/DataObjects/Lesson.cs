using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class Lesson : EntityData
    {
        public string Name { get; set; }

        [NotMapped]
        public ProgressModel Progress { get; set; }

        // parent Section FK
        public String SectionId { get; set; }

        // parent Section link
        public Section Section { get; set; }

        // parent Section2 FK
        public String Section2Id { get; set; }

        // parent Section2 link
        public Section2 Section2 { get; set; }

        // parent Section3 FK
        public String Section3Id { get; set; }

        // parent Section3 link
        public Section3 Section3 { get; set; }

        //children Exercises
        public List<Exercise> Exercises { get; set; }

    }
}