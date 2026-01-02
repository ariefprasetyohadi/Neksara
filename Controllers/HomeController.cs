using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neksara.Data;

namespace Neksara.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            AppDbContext context,
            ILogger<HomeController> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Learning()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
