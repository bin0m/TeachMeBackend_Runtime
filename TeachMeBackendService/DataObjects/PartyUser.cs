using System;
using Microsoft.Azure.Mobile.Server;



namespace TeachMeBackendService.DataObjects
{
    public class PartyUser : EntityData
    {
        // parent Party FK
        public String PartyId { get; set; }

        // parent Party link
        public Party Party { get; set; }

        // parent User FK
        public String UserId { get; set; }

        // parent User link
        public User User { get; set; }
    }
}