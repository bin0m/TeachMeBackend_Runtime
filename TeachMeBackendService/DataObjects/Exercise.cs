using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeachMeBackendService.DataObjects
{
    public class Exercise : EntityData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Image { get; set; }

        public string Sound { get; set; }

        public string Text { get; set; }

        // parent Lesson FK
        public String LessonId { get; set; }

        // parent Lesson link
        public Lesson Lesson { get; set; }

        // Children ExerciseStudents
        public List<ExerciseStudent> ExerciseStudents { get; set; }

        // Children Comments
        public List<Comment> Comments { get; set; }

        // Children Answers
        public List<Answer> Answers { get; set; }

        // Children Pairs
        public List<Pair> Pairs { get; set; }

        // Children Spaces
        public List<Space> Spaces { get; set; }
    }
}