using Microsoft.Azure.Mobile.Server;

namespace TeachMeBackendService.DataObjects
{
    public class Space : EntityData
    {
        public string Value { get; set; }

        // parent Exercise FK
        public string ExerciseId { get; set; }

        // parent Exercise link
        public Exercise Exercise { get; set; }     
    }
}