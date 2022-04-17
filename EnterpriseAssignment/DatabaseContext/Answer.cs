using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Answer
    {
        public int IdAnswer { get; set; }
        public int? IdSession { get; set; }
        public int? IdCategory { get; set; }
        public int? IdQuestion { get; set; }
        public string AnswerString { get; set; }
    }
}
