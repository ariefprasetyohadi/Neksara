using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neksara.Data;
using Neksara.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neksara.Web.Controllers
{
    public class TopicController : Controller
    {
        private readonly AppDbContext _context;

        public TopicController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // ADMIN - LIST TOPIC
        // =========================
        public async Task<IActionResult> Index()
        {
            var topics = await _context.Topics
                .Include(t => t.Category)
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(topics);
        }

        // =========================
        // ADMIN - CREATE
        // =========================
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.CreatedAt = DateTime.Now;
                topic.ViewCount = 0;
                topic.IsDeleted = false;

                _context.Topics.Add(topic);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(topic);
        }

        // =========================
        // ADMIN - EDIT
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null || topic.IsDeleted)
                return NotFound();

            ViewBag.Categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Topic topic)
        {
            if (id != topic.TopicId)
                return BadRequest();

            var existingTopic = await _context.Topics.FindAsync(id);

            if (existingTopic == null)
                return NotFound();

            existingTopic.TopicName = topic.TopicName;
            existingTopic.Body = topic.Body;
            existingTopic.VideoUrl = topic.VideoUrl;
            existingTopic.PictTopic = topic.PictTopic;
            existingTopic.IdCategory = topic.IdCategory;
            existingTopic.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // ADMIN - DELETE (SOFT)
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null || topic.IsDeleted)
                return NotFound();

            return View(topic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
                return NotFound();

            topic.IsDeleted = true;
            topic.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // USER - DETAIL TOPIC
        // AUTO VIEW COUNTER
        // =========================
        public async Task<IActionResult> Detail(int id)
        {
            var topic = await _context.Topics
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.TopicId == id && !t.IsDeleted);

            if (topic == null)
                return NotFound();

            // Increment View Count
            topic.ViewCount += 1;

            // Simpan log view (User optional)
            var topicView = new TopicView
            {
                IdTopic = topic.TopicId,
                IdUser = null, // karena belum ada login
                ViewAt = DateTime.Now
            };

            _context.TopicViews.Add(topicView);
            await _context.SaveChangesAsync();

            return View(topic);
        }
    }
}