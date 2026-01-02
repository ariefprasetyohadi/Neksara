using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neksara.Data;

namespace Neksara.Controllers
{
    public class ELearningController : Controller
    {
        private readonly AppDbContext _context;

        public ELearningController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var categories = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                categories = categories
                    .Where(c => c.CategoryName.Contains(search));
            }

            return View(await categories.ToListAsync());
        }
    }
}