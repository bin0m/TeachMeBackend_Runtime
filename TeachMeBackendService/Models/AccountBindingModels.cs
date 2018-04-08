using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TeachMeBackendService.Models
{
    public class UserSignUpModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [StringLength(100)]
        [Required]
        public string UserName { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        [StringLength(100)]
        public string AvatarPath { get; set; }

        [Required]
        public DataObjects.UserRole Role { get; set; }

    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }  

    public class LoginResult
    {
        [JsonProperty(PropertyName = "authenticationToken")]
        public string AuthenticationToken { get; set; }

        [JsonProperty(PropertyName = "user")]
        public DataObjects.User User { get; set; }
    }

    public class ClaimsUserInfo
    {
        public string Sid { get; set; }
        public string Role { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}