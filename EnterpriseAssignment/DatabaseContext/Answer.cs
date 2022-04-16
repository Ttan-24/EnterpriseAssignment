using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Answer
    {
        public int Idanswer { get; set; }
        public int? Idsession { get; set; }
        public int? Idcategory { get; set; }
        public int? Idquestion { get; set; }
        public string AnswerCharacter { get; set; }
    }
}
