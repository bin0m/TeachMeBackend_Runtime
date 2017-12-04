using System;
using Microsoft.Azure.Mobile.Server;


namespace TeachMeBackendService.DataObjects
{
    public class Course : EntityData
    {
        public string Name { get; set; }
        public string Days { get; set; }
    }
}