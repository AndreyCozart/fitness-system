using FitnessSystem.Data;
using FitnessSystem.Models;
using FitnessSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessSystem.Controllers
{
    [AuthorizeFilter]
    public class MembershipManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MembershipManagement
        public async Task<IActionResult> Index()
        {
            // Получаем все типы абонементов
            var membershipTypes = await _context.MembershipTypes
                .OrderBy(t => t.Price)
                .ToListAsync();

            // Получаем все абонементы клиентов
            var memberships = await _context.Memberships
                .Include(m => m.Client)
                .Include(m => m.Type)
                .OrderByDescending(m => m.PurchaseDate)
                .ToListAsync();

            ViewBag.MembershipTypes = membershipTypes;
            ViewBag.Memberships = memberships;

            return View();
        }

        // ==================== ТИПЫ АБОНЕМЕНТОВ ====================

        // GET: MembershipManagement/CreateType
        public IActionResult CreateType()
        {
            return View();
        }

        // POST: MembershipManagement/CreateType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateType([Bind("TypeName,VisitsCount,DurationDays,Price,Description,IsActive")] MembershipType membershipType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membershipType);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Тип абонемента '{membershipType.TypeName}' успешно добавлен";
                return RedirectToAction(nameof(Index));
            }
            return View(membershipType);
        }

        // GET: MembershipManagement/EditType/5
        public async Task<IActionResult> EditType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
            {
                return NotFound();
            }
            return View(membershipType);
        }

        // POST: MembershipManagement/EditType/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditType(int id, [Bind("Id,TypeName,VisitsCount,DurationDays,Price,Description,IsActive")] MembershipType membershipType)
        {
            if (id != membershipType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membershipType);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Тип абонемента '{membershipType.TypeName}' успешно обновлен";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipTypeExists(membershipType.Id))
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
            return View(membershipType);
        }

        // GET: MembershipManagement/DeleteType/5
        public async Task<IActionResult> DeleteType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipType = await _context.MembershipTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (membershipType == null)
            {
                return NotFound();
            }

            // Проверяем, есть ли абонементы этого типа
            var hasMemberships = await _context.Memberships.AnyAsync(m => m.TypeId == id);
            if (hasMemberships)
            {
                TempData["Error"] = "Нельзя удалить тип абонемента, который используется в абонементах клиентов";
                return RedirectToAction(nameof(Index));
            }

            return View(membershipType);
        }

        // POST: MembershipManagement/DeleteType/5
        [HttpPost, ActionName("DeleteType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTypeConfirmed(int id)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType != null)
            {
                _context.MembershipTypes.Remove(membershipType);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Тип абонемента '{membershipType.TypeName}' удален";
            }
            return RedirectToAction(nameof(Index));
        }

        // ==================== АБОНЕМЕНТЫ КЛИЕНТОВ ====================

        // GET: MembershipManagement/CreateMembership
        public async Task<IActionResult> CreateMembership(int? clientId)
        {
            ViewBag.Clients = new SelectList(await _context.Clients.ToListAsync(), "Id", "LastName", clientId);
            ViewBag.MembershipTypes = await _context.MembershipTypes.Where(t => t.IsActive).ToListAsync();

            if (clientId.HasValue)
            {
                ViewBag.SelectedClientId = clientId.Value;
                var client = await _context.Clients.FindAsync(clientId);
                if (client != null)
                {
                    ViewBag.ClientName = $"{client.LastName} {client.FirstName}";
                }
            }

            return View();
        }

        // POST: MembershipManagement/CreateMembership
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMembership(int clientId, int typeId)
        {
            var client = await _context.Clients.FindAsync(clientId);
            var type = await _context.MembershipTypes.FindAsync(typeId);

            if (client == null || type == null)
            {
                return NotFound();
            }

            var membership = new Membership
            {
                ClientId = clientId,
                TypeId = typeId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(type.DurationDays),
                VisitsRemaining = type.VisitsCount ?? -1,
                Status = "active",
                PurchaseDate = DateTime.Now
            };

            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Абонемент '{type.TypeName}' успешно оформлен для клиента {client.LastName} {client.FirstName}";

            return RedirectToAction(nameof(Index));
        }

        // GET: MembershipManagement/DetailsMembership/5
        public async Task<IActionResult> DetailsMembership(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .Include(m => m.Client)
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: MembershipManagement/FreezeMembership/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FreezeMembership(int id, int freezeDays)
        {
            var membership = await _context.Memberships.FindAsync(id);

            if (membership == null)
            {
                return NotFound();
            }

            if (membership.Status != "active")
            {
                TempData["Error"] = "Можно заморозить только активный абонемент";
                return RedirectToAction(nameof(Index));
            }

            membership.Status = "frozen";
            membership.FreezeDays = freezeDays;
            membership.EndDate = membership.EndDate.AddDays(freezeDays);

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Абонемент заморожен на {freezeDays} дней";

            return RedirectToAction(nameof(Index));
        }

        // POST: MembershipManagement/UnfreezeMembership/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnfreezeMembership(int id)
        {
            var membership = await _context.Memberships.FindAsync(id);

            if (membership == null)
            {
                return NotFound();
            }

            membership.Status = "active";
            await _context.SaveChangesAsync();
            TempData["Success"] = "Заморозка снята";

            return RedirectToAction(nameof(Index));
        }

        private bool MembershipTypeExists(int id)
        {
            return _context.MembershipTypes.Any(e => e.Id == id);
        }
    }
}