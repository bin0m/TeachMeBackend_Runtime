using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeachMeBackendService.DataObjects
{
    [Table("Section2s")]
    public class Section2 : EntityData
    {
        public string Name { get; set; }

        // parent Section FK
        public String SectionId { get; set; }

        // parent Section link
        public Section Section { get; set; }

        //children Section3s
        public List<Section3> Section3s { get; set; }

        //children Lessons
        public List<Lesson> Lessons { get; set; }
    }
}