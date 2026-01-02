using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Neksara.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Neksara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // DASHBOARD
        // =========================
        public async Task<IActionResult> Index()
        {
            var dashboardData = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(c => !c.IsDeleted),
                TotalTopics = await _context.Topics.CountAsync(t => !t.IsDeleted),
                TotalFeedbacks = await _context.Feedbacks.CountAsync(),
                PendingFeedbacks = await _context.Feedbacks.CountAsync(f => !f.IsApproved),

                PopularTopics = await _context.Topics
                    .Where(t => !t.IsDeleted)
                    .OrderByDescending(t => t.ViewCount)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardData);
        }

        // =========================
        // FEEDBACK MODERATION
        // =========================
        public async Task<IActionResult> FeedbackPending()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Where(f => !f.IsApproved)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View(feedbacks);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound();

            feedback.IsApproved = true;
            feedback.IsVisible = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(FeedbackPending));
        }

        [HttpPost]
        public async Task<IActionResult> HideFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound();

            feedback.IsVisible = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(FeedbackPending));
        }
    }
}
