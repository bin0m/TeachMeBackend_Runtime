using System;
using Microsoft.Azure.Mobile.Server;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeachMeBackendService.DataObjects
{
    public enum UserRole {
        Student = 1,
        Teacher = 2,
        Admin= 3
    }

    public class User : EntityData
    {
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
        public string Salt { get; set; }

        public int CompletedСoursesCount { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        [Required]
        public UserRole UserRole { get; set; }

       
    }
}