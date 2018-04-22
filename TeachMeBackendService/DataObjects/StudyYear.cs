﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class StudyYear : EntityData
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}