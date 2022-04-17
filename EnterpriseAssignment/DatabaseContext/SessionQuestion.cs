using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Sessionquestion
    {
        public int IdSessionQuestion { get; set; }
        public int? IdSession { get; set; }
        public int? IdQuestion { get; set; }
        public int OrderIndex { get; set; }
        public bool TextHintUsed { get; set; }
        public bool LocationHintUsed { get; set; }
    }
}
