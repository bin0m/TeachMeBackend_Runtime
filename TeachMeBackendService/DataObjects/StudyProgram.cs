using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class StudyProgram : EntityData
    {
        public string Name { get; set; }

        // many-to-many relationship
        public List<Group> Groups { get; set; }

        // many-to-many relationship
        public List<Course> Courses { get; set; }
    }
}