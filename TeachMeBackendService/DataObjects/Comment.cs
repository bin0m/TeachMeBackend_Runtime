using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeachMeBackendService.DataObjects
{
    public class Comment : EntityData
    {
        public String CommentText { get; set; }

        // Exercise FK
        public String ExerciseId { get; set; }

        // parent Exercise link
        public Exercise Exercise { get; set; }

        // parent User FK
        public String UserId { get; set; }

        // parent user link
        public User User { get; set; }

        // Children CommentRatings
        public List<CommentRating> CommentRatings { get; set; }
    }
}