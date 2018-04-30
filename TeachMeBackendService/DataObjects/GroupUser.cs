using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeachMeBackendService.Models;


namespace TeachMeBackendService.DataObjects
{
    public class GroupUser : EntityData
    {
        // parent Group FK
        public String GroupId { get; set; }

        // parent Group link
        public Group Group { get; set; }

        // parent User FK
        public String UserId { get; set; }

        // parent User link
        public User User { get; set; }
    }
}