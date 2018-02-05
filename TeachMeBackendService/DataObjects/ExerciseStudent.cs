﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeachMeBackendService.DataObjects
{
    public class ExerciseStudent : EntityData
     {
         [Required]
         public bool IsDone { get; set; }
         public String StudentAnswer { get; set; }
 
         // Exercise FK
         public String ExerciseId { get; set; }
 
         // parent Exercise link
         public Exercise Exercise { get; set; }
 
         // Student FK
         public String UserId { get; set; }
 
         // parent Student link
         public User User { get; set; }
     }
}