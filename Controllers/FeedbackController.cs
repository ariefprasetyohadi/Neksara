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
    public class FeedbackController : Controller
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // USER - CREATE FEEDBACK
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            feedback.CreatedAt = DateTime.Now;
            feedback.IsApproved = false;
            feedback.IsVisible = false;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // =========================
        // ADMIN - PENDING FEEDBACK
        // =========================
        public async Task<IActionResult> Pending()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Where(f => !f.IsApproved)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View(feedbacks);
        }

        // =========================
        // ADMIN - APPROVE
        // =========================
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound();

            feedback.IsApproved = true;
            feedback.IsVisible = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Pending));
        }

        // =========================
        // ADMIN - HIDE / REJECT
        // =========================
        [HttpPost]
        public async Task<IActionResult> Hide(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound();

            feedback.IsVisible = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Pending));
        }
    }
}
