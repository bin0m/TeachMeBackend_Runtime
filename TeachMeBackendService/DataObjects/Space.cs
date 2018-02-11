using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMeBackendService.DataObjects
{
    public class Space : EntityData
    {
        public string Value { get; set; }

        // parent Exercise FK
        public string ExerciseId { get; set; }

        // parent Exercise link
        public Exercise Exercise { get; set; }     
    }
}