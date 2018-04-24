﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    [Table("Specialties")]
    public class Specialty : EntityData
    {
        [StringLength(128)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public string Description { get; set; }

        //children Groups
        public List<Group> Groups { get; set; }
    }
}