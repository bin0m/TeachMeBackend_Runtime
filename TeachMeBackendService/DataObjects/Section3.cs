using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMeBackendService.DataObjects
{
    public class Section3 : EntityData
    {
        // parent Section2 FK
        public String Section2Id { get; set; }

        // parent Section2 link
        public Section2 Section2 { get; set; }

        //children Lessons
        public List<Lesson> Lessons { get; set; }
    }
}