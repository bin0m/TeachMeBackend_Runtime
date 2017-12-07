using System;
using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace TeachMeBackendService.DataObjects
{
    public class Course : EntityData
    {
        public string Name { get; set; }
        public int Days { get; set; }

        // Children Sections
        public List<Section> Sections { get; set; }
    }
}