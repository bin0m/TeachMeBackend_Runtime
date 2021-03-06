﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.DataObjects
{
    [Table("SectionProgresses")]
    public class SectionProgress : EntityData
     {
        public bool IsStarted { get; set; }

        public bool IsDone { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public ProgressSectionModel Progress { get; set; }

        // Section FK
        public String SectionId { get; set; }
 
         // parent Section link
         public Section Section { get; set; }
 
         // Student FK
         public String UserId { get; set; }
 
         // parent Student link
         public User User { get; set; }
     }
}