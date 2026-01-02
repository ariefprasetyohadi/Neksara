using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neksara.Data;
using Neksara.ViewModels;

namespace Neksara.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel
            {
                PopularTopics = await _context.Topics
                    .Include(t => t.Category) // ⚡ wajib biar Category tidak null
                    .Where(t => !t.IsDeleted)
                    .OrderByDescending(t => t.ViewCount)
                    .Take(5)
                    .ToListAsync()
            };

            // Bisa juga ambil categories untuk "Category Pilihan"
            vm.Categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return View(vm);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}