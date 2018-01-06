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

        // Pattern FK
        public String PatternId { get; set; }

        // parent Pattern link
        public Pattern Pattern { get; set; }

        // parent User FK
        public String UserId { get; set; }

        // parent user link
        public User User { get; set; }
    }
}