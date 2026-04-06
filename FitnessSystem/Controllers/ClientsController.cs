using FitnessSystem.Data;
using FitnessSystem.Models;
using FitnessSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessSystem.Controllers
{
    [AuthorizeFilter]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var clients = from c in _context.Clients
                          select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c =>
                    c.LastName.Contains(searchString) ||
                    c.FirstName.Contains(searchString) ||
                    c.Phone.Contains(searchString));
            }

            return View(await clients.OrderBy(c => c.LastName).ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Memberships)
                    .ThenInclude(m => m.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            // Загружаем посещения клиента
            var visits = await _context.Visits
                .Where(v => v.ClientId == id)
                .Include(v => v.Membership)
                    .ThenInclude(m => m.Type)
                .OrderByDescending(v => v.CheckInTime)
                .ToListAsync();

            ViewBag.ClientVisits = visits;

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,MiddleName,BirthDate,Gender,Phone,Email,Notes")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.RegistrationDate = DateTime.Now;
                _context.Add(client);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Клиент успешно добавлен";
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,MiddleName,BirthDate,Gender,Phone,Email,RegistrationDate,Notes")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Данные клиента обновлены";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Memberships)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Memberships)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            // Проверяем, есть ли у клиента активные абонементы
            if (client.Memberships != null && client.Memberships.Any(m => m.Status == "active" && m.EndDate >= DateTime.Today))
            {
                TempData["Error"] = "Нельзя удалить клиента с активными абонементами";
                return RedirectToAction(nameof(Index));
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Клиент удален";
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}