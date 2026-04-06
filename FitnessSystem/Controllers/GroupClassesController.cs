using FitnessSystem.Data;
using FitnessSystem.Models;
using FitnessSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessSystem.Controllers
{
    [AuthorizeFilter]
    public class GroupClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GroupClasses
        public async Task<IActionResult> Index(string dayOfWeek = "")
        {
            ViewData["CurrentDay"] = dayOfWeek;

            var classes = _context.GroupClasses.AsQueryable();

            if (!string.IsNullOrEmpty(dayOfWeek))
            {
                classes = classes.Where(c => c.DayOfWeek == dayOfWeek);
            }

            // Сначала получаем данные из базы, а потом сортируем в памяти
            var groupClasses = await classes
                .Where(c => c.IsActive)
                .ToListAsync();

            // Сортируем в памяти (клиентская сортировка)
            var daysOrder = new List<string> { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

            groupClasses = groupClasses
                .OrderBy(c => daysOrder.IndexOf(c.DayOfWeek))
                .ThenBy(c => c.StartTime)
                .ToList();

            // Для каждого занятия получаем количество записей
            var bookingsCounts = new Dictionary<int, int>();
            foreach (var item in groupClasses)
            {
                var count = await _context.GroupClassBookings
                    .CountAsync(b => b.GroupClassId == item.Id);
                bookingsCounts[item.Id] = count;
            }
            ViewBag.BookingsCounts = bookingsCounts;

            return View(groupClasses);
        }

        // GET: GroupClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupClass = await _context.GroupClasses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (groupClass == null)
            {
                return NotFound();
            }

            // Получаем все записи на это занятие
            var upcomingBookings = await _context.GroupClassBookings
                .Include(b => b.Client)
                .Where(b => b.GroupClassId == id)
                .OrderBy(b => b.ClassDate)
                .ToListAsync();

            ViewBag.UpcomingBookings = upcomingBookings;
            ViewBag.TotalBookings = upcomingBookings.Count;

            return View(groupClass);
        }

        // GET: GroupClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroupClasses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Instructor,DayOfWeek,StartTime,EndTime,MaxParticipants,Description,Price,Room,IsActive")] GroupClass groupClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupClass);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Групповое занятие '{groupClass.Name}' успешно добавлено";
                return RedirectToAction(nameof(Index));
            }
            return View(groupClass);
        }

        // GET: GroupClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupClass = await _context.GroupClasses.FindAsync(id);
            if (groupClass == null)
            {
                return NotFound();
            }
            return View(groupClass);
        }

        // POST: GroupClasses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Instructor,DayOfWeek,StartTime,EndTime,MaxParticipants,Description,Price,Room,IsActive")] GroupClass groupClass)
        {
            if (id != groupClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupClass);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Занятие '{groupClass.Name}' успешно обновлено";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupClassExists(groupClass.Id))
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
            return View(groupClass);
        }

        // GET: GroupClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupClass = await _context.GroupClasses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (groupClass == null)
            {
                return NotFound();
            }

            // Проверяем, есть ли записи на это занятие
            var hasBookings = await _context.GroupClassBookings.AnyAsync(b => b.GroupClassId == id);
            if (hasBookings)
            {
                TempData["Error"] = "Нельзя удалить занятие, на которое есть записи клиентов";
                return RedirectToAction(nameof(Index));
            }

            return View(groupClass);
        }

        // POST: GroupClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupClass = await _context.GroupClasses.FindAsync(id);
            if (groupClass != null)
            {
                _context.GroupClasses.Remove(groupClass);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Занятие '{groupClass.Name}' успешно удалено";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: GroupClasses/Book/5
        public async Task<IActionResult> Book(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupClass = await _context.GroupClasses.FindAsync(id);
            if (groupClass == null)
            {
                return NotFound();
            }

            ViewBag.GroupClass = groupClass;
            ViewBag.Clients = new SelectList(await _context.Clients.ToListAsync(), "Id", "LastName");

            return View();
        }

        // POST: GroupClasses/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(int groupClassId, int clientId, DateTime classDate)
        {
            var groupClass = await _context.GroupClasses.FindAsync(groupClassId);
            var client = await _context.Clients.FindAsync(clientId);

            if (groupClass == null || client == null)
            {
                return NotFound();
            }

            // Проверяем, не превышено ли максимальное количество участников
            var bookingsForDate = await _context.GroupClassBookings
                .CountAsync(b => b.GroupClassId == groupClassId && b.ClassDate.Date == classDate.Date);

            if (bookingsForDate >= groupClass.MaxParticipants)
            {
                TempData["Error"] = "Достигнуто максимальное количество участников";
                return RedirectToAction("Schedule");
            }

            // Проверяем, не записан ли уже клиент на эту дату
            var existingBooking = await _context.GroupClassBookings
                .FirstOrDefaultAsync(b => b.GroupClassId == groupClassId &&
                                         b.ClientId == clientId &&
                                         b.ClassDate.Date == classDate.Date);

            if (existingBooking != null)
            {
                TempData["Error"] = "Клиент уже записан на это занятие";
                return RedirectToAction("Schedule");
            }

            var booking = new GroupClassBooking
            {
                GroupClassId = groupClassId,
                ClientId = clientId,
                ClassDate = classDate,
                BookingDate = DateTime.Now
            };

            _context.GroupClassBookings.Add(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Клиент {client.LastName} {client.FirstName} успешно записан на занятие '{groupClass.Name}'";

            return RedirectToAction("Details", new { id = groupClassId });
        }

        // GET: GroupClasses/ManageBookings/5
        public async Task<IActionResult> ManageBookings(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupClass = await _context.GroupClasses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (groupClass == null)
            {
                return NotFound();
            }

            // Получаем все записи на это занятие
            var bookings = await _context.GroupClassBookings
                .Include(b => b.Client)
                .Where(b => b.GroupClassId == id)
                .OrderBy(b => b.ClassDate)
                .ThenBy(b => b.Client.LastName)
                .ToListAsync();

            ViewBag.GroupClass = groupClass;
            return View(bookings);
        }

        // POST: GroupClasses/RemoveBooking/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBooking(int id)
        {
            var booking = await _context.GroupClassBookings
                .Include(b => b.GroupClass)
                .Include(b => b.Client)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var groupClassId = booking.GroupClassId;
            var clientName = $"{booking.Client?.LastName} {booking.Client?.FirstName}";
            var className = booking.GroupClass?.Name;

            _context.GroupClassBookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Клиент {clientName} успешно исключен из занятия '{className}'";

            return RedirectToAction("ManageBookings", new { id = groupClassId });
        }

        // GET: GroupClasses/Schedule
        public async Task<IActionResult> Schedule(DateTime? weekStart)
        {
            var startDate = weekStart ?? DateTime.Today;
            // Находим начало недели (понедельник)
            while (startDate.DayOfWeek != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(-1);
            }

            var endDate = startDate.AddDays(6);
            ViewBag.WeekStart = startDate;
            ViewBag.WeekEnd = endDate;
            ViewBag.PrevWeek = startDate.AddDays(-7);
            ViewBag.NextWeek = startDate.AddDays(7);

            // Получаем все групповые занятия
            var allClasses = await _context.GroupClasses
                .Where(c => c.IsActive)
                .ToListAsync();

            // Создаем данные для представления
            var scheduleData = new List<dynamic>();

            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);
                var dayName = GetRussianDayName(date.DayOfWeek);

                var classesOnDay = allClasses
                    .Where(c => c.DayOfWeek == dayName)
                    .OrderBy(c => c.StartTime)
                    .Select(c => new
                    {
                        Class = c,
                        BookingsCount = _context.GroupClassBookings
                            .Count(b => b.GroupClassId == c.Id && b.ClassDate.Date == date.Date)
                    })
                    .ToList();

                scheduleData.Add(new
                {
                    Date = date,
                    DayName = dayName,
                    Classes = classesOnDay
                });
            }

            ViewBag.ScheduleData = scheduleData;
            return View();
        }

        // GET: GroupClasses/MyBookings
        public async Task<IActionResult> MyBookings(int? clientId)
        {
            if (!clientId.HasValue)
            {
                return RedirectToAction("Index", "Clients");
            }

            var client = await _context.Clients.FindAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }

            var bookings = await _context.GroupClassBookings
                .Include(b => b.GroupClass)
                .Where(b => b.ClientId == clientId)
                .OrderBy(b => b.ClassDate)
                .ToListAsync();

            ViewBag.ClientName = $"{client.LastName} {client.FirstName}";
            return View(bookings);
        }

        // POST: GroupClasses/CancelBooking/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.GroupClassBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var clientId = booking.ClientId;
            _context.GroupClassBookings.Remove(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Запись успешно отменена";

            return RedirectToAction("MyBookings", new { clientId });
        }

        private bool GroupClassExists(int id)
        {
            return _context.GroupClasses.Any(e => e.Id == id);
        }

        private string GetRussianDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => "Понедельник",
                DayOfWeek.Tuesday => "Вторник",
                DayOfWeek.Wednesday => "Среда",
                DayOfWeek.Thursday => "Четверг",
                DayOfWeek.Friday => "Пятница",
                DayOfWeek.Saturday => "Суббота",
                DayOfWeek.Sunday => "Воскресенье",
                _ => ""
            };
        }
    }
}