using EnterpriseAssignment.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseController : Controller
    {
        private readonly quiz_dbContext context;

        public EnterpriseController(quiz_dbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult GetCategories()
        {
            var categories = context.Categories.Include(c => c.QuestionList);
            var categoryList = new List<CategoryWithQuestion>();
            foreach (var category in categories)
            {
                categoryList.Add(new CategoryWithQuestion(category, category.QuestionList.Count));
            }
            return Ok(categoryList);
        }
    }

    // making a model
    public record CategoryWithQuestion(Category Categories, int QuestionCount);
}
