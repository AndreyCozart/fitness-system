using FitnessSystem.Data;
using FitnessSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessSystem.Controllers
{
    [AuthorizeFilter("Admin")] // Только для администраторов
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Logs
        public async Task<IActionResult> Logs(DateTime? date, string? username)
        {
            var query = _context.LogEntries.AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(l => l.Timestamp.Date == date.Value.Date);
            }

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(l => l.Username != null && l.Username.Contains(username));
            }

            var logs = await query
                .OrderByDescending(l => l.Timestamp)
                .Take(500)
                .ToListAsync();

            ViewBag.CurrentDate = date?.ToString("yyyy-MM-dd") ?? "";
            ViewBag.CurrentUsername = username ?? "";

            return View(logs);
        }

        // POST: Admin/ClearLogs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearLogs()
        {
            // Удаляем логи старше 30 дней
            var oldDate = DateTime.Now.AddDays(-30);
            var oldLogs = await _context.LogEntries
                .Where(l => l.Timestamp < oldDate)
                .ToListAsync();

            if (oldLogs.Any())
            {
                _context.LogEntries.RemoveRange(oldLogs);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Удалено {oldLogs.Count} записей логов старше 30 дней";
            }
            else
            {
                TempData["Info"] = "Нет логов для удаления";
            }

            return RedirectToAction("Logs");
        }
    }
}