using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Neksara.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Neksara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFeedbackController : Controller
    {
        private readonly AppDbContext _context;

        public AdminFeedbackController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // LIST PENDING FEEDBACK
        // =========================
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Where(f => !f.IsApproved)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View(feedbacks);
        }

        // =========================
        // APPROVE FEEDBACK
        // =========================
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound();

            feedback.IsApproved = true;
            feedback.IsVisible = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // HIDE / REJECT FEEDBACK
        // =========================
        [HttpPost]
        public async Task<IActionResult> Hide(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound();

            feedback.IsVisible = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
