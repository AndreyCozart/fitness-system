using FitnessSystem.Data;
using FitnessSystem.Models;
using FitnessSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessSystem.Controllers
{
    [AuthorizeFilter]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Reports/Visits
        public async Task<IActionResult> Visits(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? DateTime.Today.AddDays(-30);
            var end = endDate ?? DateTime.Today;

            var visits = await _context.Visits
                .Where(v => v.CheckInTime.Date >= start.Date && v.CheckInTime.Date <= end.Date)
                .ToListAsync();

            var report = new VisitsReportViewModel
            {
                StartDate = start,
                EndDate = end,
                TotalVisits = visits.Count,
                UniqueClients = visits.Select(v => v.ClientId).Distinct().Count(),
                DailyVisits = visits
                    .GroupBy(v => v.CheckInTime.Date)
                    .Select(g => new DailyVisits { Date = g.Key, Count = g.Count() })
                    .OrderBy(g => g.Date)
                    .ToList(),
                HourlyLoad = Enumerable.Range(8, 12)
                    .Select(h => new HourlyLoad
                    {
                        Hour = h,
                        AverageVisits = visits.Count(v => v.CheckInTime.Hour == h)
                    })
                    .ToList()
            };

            return View(report);
        }

        // GET: Reports/Revenue
        public async Task<IActionResult> Revenue(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? DateTime.Today.AddDays(-30);
            var end = endDate ?? DateTime.Today;

            var memberships = await _context.Memberships
                .Include(m => m.Type)
                .Where(m => m.PurchaseDate.Date >= start.Date && m.PurchaseDate.Date <= end.Date)
                .ToListAsync();

            var report = new RevenueReportViewModel
            {
                StartDate = start,
                EndDate = end,
                TotalRevenue = memberships.Sum(m => m.Type?.Price ?? 0),
                RevenueByMembershipType = memberships
                    .GroupBy(m => m.Type?.TypeName ?? "Неизвестно")
                    .OrderByDescending(g => g.Sum(m => m.Type?.Price ?? 0))
                    .ToDictionary(g => g.Key, g => g.Sum(m => m.Type?.Price ?? 0))
            };

            return View(report);
        }

        // GET: Reports/GroupClasses
        public async Task<IActionResult> GroupClasses(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? DateTime.Today.AddDays(-30);
            var end = endDate ?? DateTime.Today;

            var bookings = await _context.GroupClassBookings
                .Include(b => b.GroupClass)
                .Where(b => b.ClassDate.Date >= start.Date && b.ClassDate.Date <= end.Date)
                .ToListAsync();

            var report = new GroupClassesReportViewModel
            {
                StartDate = start,
                EndDate = end,
                PopularityByClass = bookings
                    .GroupBy(b => b.GroupClass?.Name ?? "Неизвестно")
                    .OrderByDescending(g => g.Count())
                    .ToDictionary(g => g.Key, g => g.Count()),
                PopularityByInstructor = bookings
                    .GroupBy(b => b.GroupClass?.Instructor ?? "Неизвестно")
                    .OrderByDescending(g => g.Count())
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return View(report);
        }
    }
}