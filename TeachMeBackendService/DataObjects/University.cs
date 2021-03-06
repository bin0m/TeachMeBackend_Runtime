﻿using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TeachMeBackendService.DataObjects
{
    [Table("Universities")]
    public class University : EntityData
    {
        [StringLength(128)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [StringLength(16)]
        public string ShortName { get; set; }

        public string Description { get; set; }

        //children Parties
        public List<Party> Parties { get; set; }
    }
}