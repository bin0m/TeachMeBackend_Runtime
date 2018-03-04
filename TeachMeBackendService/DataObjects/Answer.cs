using Microsoft.Azure.Mobile.Server;

namespace TeachMeBackendService.DataObjects
{
    public class Answer : EntityData
    {
        public string Title { get; set; }

        public bool IsRight { get; set; }

        // parent Exercise FK
        public string ExerciseId { get; set; }

        // parent Exercise link
        public Exercise Exercise { get; set; }     
    }
}