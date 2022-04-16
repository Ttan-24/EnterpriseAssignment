using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Session
    {
        public int Idsession { get; set; }
        public string Name { get; set; }
        public int? Idcategory { get; set; }
        public int? Score { get; set; }
        public string UnusedColumn { get; set; }
        public int CurrentQuestionIndex { get; set; }
    }
}
