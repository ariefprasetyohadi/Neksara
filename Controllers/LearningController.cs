using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neksara.Data;

public class LearningController : Controller
{
    private readonly AppDbContext _context;
    private const int PageSize = 6;

    public LearningController(AppDbContext context)
    {
        _context = context;
    }

    // ================= LIST + FILTER + PAGINATION =================
    public async Task<IActionResult> Index(int? categoryId, int page = 1)
    {
        var categories = await _context.Categories
            .Where(c => !c.IsDeleted)
            .ToListAsync();

        var query = _context.Topics
            .Include(t => t.Category)
            .Where(t => !t.IsDeleted);

        if (categoryId != null)
        {
            query = query.Where(t => t.IdCategory == categoryId);
        }

        var totalItems = await query.CountAsync();

        var topics = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.CurrentCategory = categoryId;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

        return View(topics);
    }

    // ================= DETAIL TOPIK =================
    public async Task<IActionResult> Detail(int id)
    {
        var topic = await _context.Topics
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TopicId == id && !t.IsDeleted);

        if (topic == null)
            return NotFound();

        topic.ViewCount += 1;
        await _context.SaveChangesAsync();

        return View(topic);
    }
}
