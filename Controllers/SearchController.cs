using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Neksara.Data;
using Neksara.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neksara.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        private const int CACHE_DURATION = 60;

        public SearchController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // =========================
        // USER - SEARCH
        // =========================
        public async Task<IActionResult> Index(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return View(null);

            var cacheKey = $"search_{keyword.ToLower()}";

            var result = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CACHE_DURATION);

                var categories = await _context.Categories
                    .Where(c => !c.IsDeleted && c.CategoryName.Contains(keyword))
                    .ToListAsync();

                var topics = await _context.Topics
                    .Where(t => !t.IsDeleted &&
                               ((t.TopicName != null && t.TopicName.Contains(keyword)) ||
                                (t.Body != null && t.Body.Contains(keyword))))
                    .ToListAsync();

                return new
                {
                    Categories = categories,
                    Topics = topics
                };
            });

            // Simpan Search Log
            var log = new SearchLog
            {
                Keyword = keyword,
                ResultCount = ((result?.Categories?.Count) ?? 0) + ((result?.Topics?.Count) ?? 0),
                SearchedAt = DateTime.Now
            };

            _context.SearchLogs.Add(log);
            await _context.SaveChangesAsync();

            ViewBag.Keyword = keyword;
            ViewBag.Categories = result?.Categories;
            ViewBag.Topics = result?.Topics;

            return View();
        }
    }
}