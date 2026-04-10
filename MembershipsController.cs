using FitnessSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessSystem.Controllers
{
    public class MembershipsController : Controller
    {
        // Временное хранилище данных
        public static List<Membership> _memberships = new List<Membership>();
        private static List<Client> _clients = ClientsController._clients;
        private static List<MembershipType> _membershipTypes = MembershipTypesController._membershipTypes;

        // GET: Memberships
        public IActionResult Index()
        {
            return View(_memberships);
        }

        // GET: Memberships/Create
        public IActionResult Create(int? clientId)
        {
            ViewBag.Clients = new SelectList(_clients, "Id", "LastName", clientId);
            ViewBag.MembershipTypes = new SelectList(_membershipTypes.Where(t => t.IsActive), "Id", "TypeName");
            return View();
        }

        // POST: Memberships/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int clientId, int typeId)
        {
            var client = _clients.FirstOrDefault(c => c.Id == clientId);
            var type = _membershipTypes.FirstOrDefault(t => t.Id == typeId);

            if (client == null || type == null)
            {
                return NotFound();
            }

            var membership = new Membership
            {
                Id = _memberships.Count + 1,
                ClientId = clientId,
                TypeId = typeId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(type.DurationDays),
                VisitsRemaining = type.VisitsCount ?? -1,
                Status = "active",
                PurchaseDate = DateTime.Now
            };

            _memberships.Add(membership);
            TempData["Success"] = "Абонемент успешно создан";

            return RedirectToAction("Details", "Clients", new { id = clientId });
        }

        // GET: Memberships/Edit/5
        public IActionResult Edit(int id)
        {
            var membership = _memberships.FirstOrDefault(m => m.Id == id);
            if (membership == null)
            {
                return NotFound();
            }
            return View(membership);
        }

        // GET: Memberships/Details/5
        public IActionResult Details(int id)
        {
            var membership = _memberships.FirstOrDefault(m => m.Id == id);
            if (membership == null)
            {
                return NotFound();
            }
            return View(membership);
        }
    }
}