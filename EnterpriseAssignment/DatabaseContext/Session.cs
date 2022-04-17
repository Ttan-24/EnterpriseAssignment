using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Session
    {
        public int IdSession { get; set; }
        public int? IdCategory { get; set; }
        public string AppId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int CurrentQuestionIndex { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public int QuestionCount { get; set; }
        public bool EndSession { get; set; }
    }
}
