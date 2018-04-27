using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class StudyPeriod : EntityData
    {
        [StringLength(128)]
        public string Name { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        //children Groups
        public List<Group> Groups { get; set; }

    }
}