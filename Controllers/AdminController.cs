using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Neksara.Data;
using Neksara.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neksara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private const int PAGE_SIZE = 5;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // DASHBOARD
        // =========================
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // CATEGORY CRUD
        // =========================
        public async Task<IActionResult> Categories(string search, int page = 1)
        {
            var query = _context.Categories.Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.CategoryName.Contains(search));

            int totalData = await query.CountAsync();

            var data = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = Math.Ceiling(totalData / (double)PAGE_SIZE);
            ViewBag.Search = search;

            return View(data);
        }

        public IActionResult CreateCategory() => View();

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            model.CreatedAt = DateTime.Now;
            model.IsDeleted = false;

            _context.Categories.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var data = await _context.Categories.FindAsync(id);
            if (data == null) return NotFound();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Categories.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var data = await _context.Categories.FindAsync(id);
            if (data == null) return NotFound();

            data.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }

        // =========================
        // TOPIC CRUD
        // =========================
        public async Task<IActionResult> Topics(string search, int page = 1)
        {
            var query = _context.Topics
                .Include(t => t.Category)
                .Where(t => !t.IsDeleted);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.TopicName.Contains(search));

            int totalData = await query.CountAsync();

            var data = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = Math.Ceiling(totalData / (double)PAGE_SIZE);
            ViewBag.Search = search;

            return View(data);
        }

        public async Task<IActionResult> CreateTopic()
        {
            ViewBag.Categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(Topic model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Categories = await _context.Categories
                    .Where(c => !c.IsDeleted)
                    .ToListAsync();
                    
                return View(model);
            }

            model.CreatedAt = DateTime.Now;
            model.ViewCount = 0;
            model.IsDeleted = false;

            _context.Topics.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Topics));
        }

        public async Task<IActionResult> EditTopic(int id)
        {
            var data = await _context.Topics.FindAsync(id);
            if (data == null) return NotFound();

            ViewBag.Categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> EditTopic(Topic model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(model);
            }

            model.UpdatedAt = DateTime.Now;

            _context.Topics.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Topics));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var data = await _context.Topics.FindAsync(id);
            if (data == null) return NotFound();

            data.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Topics));
        }
    }
}
