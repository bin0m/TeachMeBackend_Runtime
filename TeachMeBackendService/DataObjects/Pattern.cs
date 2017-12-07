using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMeBackendService.DataObjects
{
    public class Pattern : EntityData
    {
        public string Name { get; set; }
        public string JsonText { get; set; }

        //link to Lessons where Pattern is Used
        public List<Lesson> Lessons { get; set; }
    }
}