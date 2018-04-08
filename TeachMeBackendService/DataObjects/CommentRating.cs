using Microsoft.Azure.Mobile.Server;
using System;
using System.ComponentModel.DataAnnotations;

namespace TeachMeBackendService.DataObjects
{
    public class CommentRating : EntityData
    {
        public int Rating { get; set; }

        // Comment FK
        [Required]
        public String CommentId { get; set; }

        // parent Comment link
        public Comment Comment { get; set; }

        // parent User FK
        [Required]
        public String UserId { get; set; }

        // parent user link
        public User User { get; set; }
    }
}