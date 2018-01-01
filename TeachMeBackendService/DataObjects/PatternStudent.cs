using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeachMeBackendService.DataObjects
{
    public class PatternStudent : EntityData
    {
        [Required]
        public bool IsDone { get; set; }
        public String StudentAnswer { get; set; }

        // Pattern FK
        public String PatternId { get; set; }

        // parent Pattern link
        public Pattern Pattern { get; set; }

        // Student FK
        public String UserId { get; set; }

        // parent Student link
        public User User { get; set; }
    }
}