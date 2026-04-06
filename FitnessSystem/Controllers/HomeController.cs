using FitnessSystem.Data;
using FitnessSystem.Models;
using FitnessSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FitnessSystem.Controllers
{
    [AuthorizeFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var today = DateTime.Today;

                var viewModel = new DashboardViewModel
                {
                    TotalClients = await _context.Clients.CountAsync(),
                    ActiveMemberships = await _context.Memberships
                        .CountAsync(m => m.Status == "active" && m.EndDate >= today),
                    TodayVisits = await _context.Visits
                        .CountAsync(v => v.CheckInTime.Date == today),
                    ClientsInGym = await _context.Visits
                        .CountAsync(v => v.CheckOutTime == null),

                    RecentClients = await _context.Clients
                        .OrderByDescending(c => c.RegistrationDate)
                        .Take(5)
                        .Select(c => new RecentClient
                        {
                            Id = c.Id,
                            FullName = c.LastName + " " + c.FirstName + " " + (c.MiddleName ?? ""),
                            Phone = c.Phone,
                            RegistrationDate = c.RegistrationDate
                        })
                        .ToListAsync(),

                    ExpiringMemberships = await _context.Memberships
                        .Include(m => m.Client)
                        .Include(m => m.Type)
                        .Where(m => m.Status == "active" && m.EndDate <= today.AddDays(7))
                        .OrderBy(m => m.EndDate)
                        .Take(5)
                        .Select(m => new ExpiringMembership
                        {
                            ClientId = m.ClientId,
                            ClientName = m.Client.LastName + " " + m.Client.FirstName,
                            MembershipType = m.Type.TypeName,
                            EndDate = m.EndDate,
                            DaysLeft = (m.EndDate - today).Days
                        })
                        .ToListAsync(),

                    VisitsByDay = await _context.Visits
                        .Where(v => v.CheckInTime >= today.AddDays(-30))
                        .GroupBy(v => v.CheckInTime.Date)
                        .Select(g => new VisitStatistics
                        {
                            Date = g.Key,
                            Count = g.Count()
                        })
                        .OrderBy(v => v.Date)
                        .ToListAsync(),

                    PopularMembershipTypes = await _context.MembershipTypes
                        .Select(t => new PopularMembershipType
                        {
                            TypeName = t.TypeName,
                            Count = t.Memberships.Count
                        })
                        .OrderByDescending(t => t.Count)
                        .Take(5)
                        .ToListAsync()
                };

                ViewBag.PopularClasses = await _context.GroupClasses
                    .Where(g => g.IsActive)
                    .OrderBy(g => g.Name)
                    .Take(6)
                    .ToListAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке главной страницы");

                var emptyViewModel = new DashboardViewModel
                {
                    TotalClients = 0,
                    ActiveMemberships = 0,
                    TodayVisits = 0,
                    ClientsInGym = 0,
                    RecentClients = new List<RecentClient>(),
                    ExpiringMemberships = new List<ExpiringMembership>(),
                    VisitsByDay = new List<VisitStatistics>(),
                    PopularMembershipTypes = new List<PopularMembershipType>()
                };

                ViewBag.PopularClasses = new List<GroupClass>();

                return View(emptyViewModel);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}