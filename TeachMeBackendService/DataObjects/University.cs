using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    [Table("Universities")]
    public class University : EntityData
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        //children Groups
        public List<Group> Groups { get; set; }
    }
}