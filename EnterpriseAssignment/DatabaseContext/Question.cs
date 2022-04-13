using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Question
    {
        public int Idquestion { get; set; }
        public int? Idcategory { get; set; }
        public string Prompt { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string CorrectAnswer { get; set; }

        public Category Category;
    }
}
