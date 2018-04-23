using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    [Table("Faculties")]
    public class Faculty : EntityData
    {
        public string Name { get; set; }

        //children Groups
        public List<Group> Groups { get; set; }
    }
}