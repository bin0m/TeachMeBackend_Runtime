using System;
using Microsoft.Azure.Mobile.Server;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeachMeBackendService.DataObjects
{
    public class Student : EntityData
    {
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int CompletedСoursesCount { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}