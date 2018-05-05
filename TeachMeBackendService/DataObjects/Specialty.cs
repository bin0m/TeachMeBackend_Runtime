using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TeachMeBackendService.DataObjects
{
    [Table("Specialties")]
    public class Specialty : EntityData
    {
        [StringLength(128)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public string Description { get; set; }

        //children Parties
        public List<Party> Parties { get; set; }
    }
}