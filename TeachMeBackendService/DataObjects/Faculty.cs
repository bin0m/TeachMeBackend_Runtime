using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    [Table("Faculties")]
    public class Faculty : EntityData
    {
        [StringLength(128)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        //children Groups
        public List<Group> Groups { get; set; }
    }
}