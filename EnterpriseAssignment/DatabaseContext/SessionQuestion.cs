using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class SessionQuestion
    {
        public int Idsessionquestion { get; set; }
        public int? Idsession { get; set; }
        public int? Idquestion { get; set; }
        public int OrderIndex { get; set; }
    }
}
