using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Session
    {
        public int Iduser { get; set; }
        public string Name { get; set; }
        public int? Idcategory { get; set; }
        public int? Score { get; set; }
        public string CurrentQuestion { get; set; }
    }
}
