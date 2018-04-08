using Microsoft.Azure.Mobile.Server;

namespace TeachMeBackendService.DataObjects
{
    public class Pair : EntityData
    {
        public string Value { get; set; }

        public string Equal { get; set; }

        // parent Exercise FK
        public string ExerciseId { get; set; }

        // parent Exercise link
        public Exercise Exercise { get; set; }     
    }
}