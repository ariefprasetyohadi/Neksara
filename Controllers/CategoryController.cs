using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neksara.Models;
using Neksara.Data; // Add this if AppDbContext is in Neksara.Data namespace
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neksara.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: Category
        // =========================
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(categories);
        }

        // =========================
        // GET: Category/Create
        // =========================
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // POST: Category/Create
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedAt = DateTime.Now;
                category.IsDeleted = false;

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // =========================
        // GET: Category/Edit/5
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.IsDeleted)
                return NotFound();

            return View(category);
        }

        // =========================
        // POST: Category/Edit/5
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoriesId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Categories.FindAsync(id);

                if (existingCategory == null)
                    return NotFound();

                existingCategory.CategoryName = category.CategoryName;
                existingCategory.Description = category.Description;
                existingCategory.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // =========================
        // GET: Category/Delete/5
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.IsDeleted)
                return NotFound();

            return View(category);
        }

        // =========================
        // POST: Category/Delete/5
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            category.IsDeleted = true;
            category.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
