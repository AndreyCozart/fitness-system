using FitnessSystem.Data;
using FitnessSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSystem.Controllers
{
    public class MembershipTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MembershipTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MembershipTypes.ToListAsync());
        }

        // GET: MembershipTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MembershipTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeName,VisitsCount,DurationDays,Price,Description,IsActive")] MembershipType membershipType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membershipType);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Тип абонемента добавлен";
                return RedirectToAction(nameof(Index));
            }
            return View(membershipType);
        }

        // GET: MembershipTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: MembershipTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeName,VisitsCount,DurationDays,Price,Description,IsActive")] MembershipType membershipType)
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
                    TempData["Success"] = "Тип абонемента обновлен";
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

        // GET: MembershipTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipType = await _context.MembershipTypes
                .Include(t => t.Memberships)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (membershipType == null)
            {
                return NotFound();
            }

            if (membershipType.Memberships.Any())
            {
                TempData["Error"] = "Нельзя удалить тип абонемента, который используется";
                return RedirectToAction(nameof(Index));
            }

            return View(membershipType);
        }

        // POST: MembershipTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType != null)
            {
                _context.MembershipTypes.Remove(membershipType);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Тип абонемента удален";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MembershipTypeExists(int id)
        {
            return _context.MembershipTypes.Any(e => e.Id == id);
        }
    }
}