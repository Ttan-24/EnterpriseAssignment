using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Question
    {
        public int IdQuestion { get; set; }
        public int? IdCategory { get; set; }
        public string Prompt { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string CorrectAnswer { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public bool hasLocation { get; set; }
        public string Hint { get; set; }
    }
}
