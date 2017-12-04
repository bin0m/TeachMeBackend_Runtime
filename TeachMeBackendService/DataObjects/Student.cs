using System;
using Microsoft.Azure.Mobile.Server;


namespace TeachMeBackendService.DataObjects
{
    public class Student : EntityData
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int CompletedСoursesCount { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}