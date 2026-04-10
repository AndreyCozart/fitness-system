using FitnessSystem.Data;
using FitnessSystem.Models;
using FitnessSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessSystem.Controllers
{
    [AuthorizeFilter]
    public class VisitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visits
        public async Task<IActionResult> Index()
        {
            var visits = await _context.Visits
                .Include(v => v.Client)
                .Include(v => v.Membership)
                    .ThenInclude(m => m.Type)
                .OrderByDescending(v => v.CheckInTime)
                .ToListAsync();

            return View(visits);
        }

        // GET: Visits/CheckIn
        public async Task<IActionResult> CheckIn()
        {
            ViewBag.Clients = new SelectList(await _context.Clients.ToListAsync(), "Id", "LastName");
            return View();
        }

        // POST: Visits/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }

            // Проверяем активный абонемент
            var activeMembership = await _context.Memberships
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.ClientId == clientId &&
                                         m.Status == "active" &&
                                         m.EndDate >= DateTime.Today &&
                                         (m.VisitsRemaining > 0 || m.VisitsRemaining == -1));

            if (activeMembership == null)
            {
                TempData["Error"] = "У клиента нет активного абонемента";
                return RedirectToAction(nameof(CheckIn));
            }

            // Проверяем, не в зале ли уже
            var activeVisit = await _context.Visits
                .FirstOrDefaultAsync(v => v.ClientId == clientId && v.CheckOutTime == null);

            if (activeVisit != null)
            {
                TempData["Error"] = "Клиент уже находится в зале";
                return RedirectToAction(nameof(CheckIn));
            }

            // Создаем посещение
            var visit = new Visit
            {
                ClientId = clientId,
                MembershipId = activeMembership.Id,
                CheckInTime = DateTime.Now
            };

            // Уменьшаем количество посещений, если абонемент не безлимитный
            if (activeMembership.VisitsRemaining > 0)
            {
                activeMembership.VisitsRemaining--;
            }

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Посещение зарегистрировано для {client.LastName} {client.FirstName}";

            return RedirectToAction(nameof(Index));
        }

        // POST: Visits/CheckOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Client)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            if (visit.CheckOutTime.HasValue)
            {
                TempData["Error"] = "Посещение уже завершено";
                return RedirectToAction(nameof(Index));
            }

            visit.CheckOutTime = DateTime.Now;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Выход зарегистрирован для {visit.Client?.LastName} {visit.Client?.FirstName}";

            return RedirectToAction(nameof(Index));
        }

        // GET: Visits/Active
        public async Task<IActionResult> Active()
        {
            var activeVisits = await _context.Visits
                .Include(v => v.Client)
                .Include(v => v.Membership)
                    .ThenInclude(m => m.Type)
                .Where(v => v.CheckOutTime == null)
                .OrderBy(v => v.CheckInTime)
                .ToListAsync();

            return View(activeVisits);
        }

        // GET: Visits/History/5
        public async Task<IActionResult> History(int? clientId)
        {
            if (!clientId.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }

            var client = await _context.Clients.FindAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }

            var visits = await _context.Visits
                .Include(v => v.Membership)
                    .ThenInclude(m => m.Type)
                .Where(v => v.ClientId == clientId)
                .OrderByDescending(v => v.CheckInTime)
                .ToListAsync();

            ViewBag.ClientName = $"{client.LastName} {client.FirstName}";
            return View(visits);
        }
    }
}