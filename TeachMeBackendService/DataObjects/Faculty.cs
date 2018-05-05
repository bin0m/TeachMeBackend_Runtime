using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TeachMeBackendService.DataObjects
{
    [Table("Faculties")]
    public class Faculty : EntityData
    {
        [StringLength(128)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        //children Parties
        public List<Party> Parties { get; set; }
    }
}