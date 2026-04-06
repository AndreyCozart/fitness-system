using FitnessSystem.Data;
using FitnessSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            // Если уже залогинен, перенаправляем в админку
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // В реальном проекте нужно проверять хешированный пароль!
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username &&
                                              u.Password == model.Password &&
                                              u.IsActive);

                if (user != null)
                {
                    // Сохраняем в сессию
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("UserRole", user.Role);

                    // Обновляем время последнего входа
                    user.LastLoginAt = DateTime.Now;
                    await _context.SaveChangesAsync();

                    // Логируем вход
                    var logEntry = new LogEntry
                    {
                        Username = user.Username,
                        Action = "Вход в систему",
                        Controller = "Account",
                        Details = "Успешный вход",
                        IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                        Timestamp = DateTime.Now
                    };
                    _context.LogEntries.Add(logEntry);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Добро пожаловать, {user.Username}!";

                    // Перенаправляем на главную страницу после успешного входа
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Неверное имя пользователя или пароль");
            }
            return View(model);
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            var username = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(username))
            {
                // Логируем выход
                var logEntry = new LogEntry
                {
                    Username = username,
                    Action = "Выход из системы",
                    Controller = "Account",
                    IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                    Timestamp = DateTime.Now
                };
                _context.LogEntries.Add(logEntry);
                await _context.SaveChangesAsync();
            }

            // Очищаем сессию
            HttpContext.Session.Clear();

            TempData["Info"] = "Вы вышли из системы";
            return RedirectToAction("Login");
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}