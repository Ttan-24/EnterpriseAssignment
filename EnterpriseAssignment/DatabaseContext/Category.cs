using System;
using System.Collections.Generic;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class Category
    {
        public int Idcategory { get; set; }
        public string CategoryName { get; set; }
        public List<Question> QuestionList;
    }
}
