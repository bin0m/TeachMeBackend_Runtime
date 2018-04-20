﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class University : EntityData
    {
        public string Name { get; set; }

        public string LongName { get; set; }

        public string Description { get; set; }
    }
}