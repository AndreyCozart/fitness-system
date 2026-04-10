using FitnessSystem.Data;
using FitnessSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalClients = await _context.Clients.CountAsync(),
                ActiveMemberships = await _context.Memberships
                    .CountAsync(m => m.Status == "active" && m.EndDate >= System.DateTime.Today),
                TodayVisits = await _context.Visits
                    .CountAsync(v => v.CheckInTime.Date == System.DateTime.Today),
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
                    .Where(m => m.Status == "active" && m.EndDate <= System.DateTime.Today.AddDays(7))
                    .OrderBy(m => m.EndDate)
                    .Take(5)
                    .Select(m => new ExpiringMembership
                    {
                        ClientId = m.ClientId,
                        ClientName = m.Client.LastName + " " + m.Client.FirstName,
                        MembershipType = m.Type.TypeName,
                        EndDate = m.EndDate,
                        DaysLeft = (m.EndDate - System.DateTime.Today).Days
                    })
                    .ToListAsync(),

                VisitsByDay = await _context.Visits
                    .Where(v => v.CheckInTime >= System.DateTime.Today.AddDays(-30))
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

            return View(viewModel);
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