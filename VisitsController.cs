using FitnessSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessSystem.Controllers
{
    public class VisitsController : Controller
    {
        // Временное хранилище данных
        private static List<Visit> _visits = new List<Visit>();
        private static List<Client> _clients = ClientsController._clients;
        private static List<Membership> _memberships = MembershipsController._memberships;

        // GET: Visits
        public IActionResult Index()
        {
            return View(_visits.OrderByDescending(v => v.CheckInTime).ToList());
        }

        // GET: Visits/CheckIn
        public IActionResult CheckIn()
        {
            ViewBag.Clients = new SelectList(_clients, "Id", "LastName");
            return View();
        }

        // POST: Visits/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckIn(int clientId)
        {
            var client = _clients.FirstOrDefault(c => c.Id == clientId);
            if (client == null)
            {
                return NotFound();
            }

            // Проверяем активный абонемент
            var activeMembership = _memberships
                .FirstOrDefault(m => m.ClientId == clientId &&
                                    m.Status == "active" &&
                                    m.EndDate >= DateTime.Today &&
                                    (m.VisitsRemaining > 0 || m.VisitsRemaining == -1));

            if (activeMembership == null)
            {
                TempData["Error"] = "У клиента нет активного абонемента";
                return RedirectToAction(nameof(CheckIn));
            }

            // Проверяем, не в зале ли уже
            var activeVisit = _visits.FirstOrDefault(v => v.ClientId == clientId && v.CheckOutTime == null);
            if (activeVisit != null)
            {
                TempData["Error"] = "Клиент уже находится в зале";
                return RedirectToAction(nameof(CheckIn));
            }

            // Создаем посещение
            var visit = new Visit
            {
                Id = _visits.Count + 1,
                ClientId = clientId,
                MembershipId = activeMembership.Id,
                CheckInTime = DateTime.Now
            };

            // Уменьшаем количество посещений
            if (activeMembership.VisitsRemaining > 0)
            {
                activeMembership.VisitsRemaining--;
            }

            _visits.Add(visit);
            TempData["Success"] = $"Посещение зарегистрировано для {client.LastName} {client.FirstName}";

            return RedirectToAction(nameof(Index));
        }

        // POST: Visits/CheckOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOut(int id)
        {
            var visit = _visits.FirstOrDefault(v => v.Id == id);
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
            TempData["Success"] = "Выход зарегистрирован";

            return RedirectToAction(nameof(Index));
        }

        // GET: Visits/Active
        public IActionResult Active()
        {
            var activeVisits = _visits.Where(v => v.CheckOutTime == null).ToList();
            return View(activeVisits);
        }

        // GET: Visits/History/5
        public IActionResult History(int clientId)
        {
            var client = _clients.FirstOrDefault(c => c.Id == clientId);
            if (client == null)
            {
                return NotFound();
            }

            var visits = _visits.Where(v => v.ClientId == clientId)
                                .OrderByDescending(v => v.CheckInTime)
                                .ToList();

            ViewBag.ClientName = $"{client.LastName} {client.FirstName}";
            return View(visits);
        }
    }
}