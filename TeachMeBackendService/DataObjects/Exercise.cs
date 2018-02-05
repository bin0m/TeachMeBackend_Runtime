﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
       
        // parent Lesson FK
        public String LessonId { get; set; }

        // parent Lesson link
        public Lesson Lesson { get; set; }

        // Children Comments
        public List<Comment> Comments { get; set; }

        // Children Answers
        public List<Answer> Answers { get; set; }

        // Children Pairs
        public List<Pair> Pairs { get; set; }
    }
}