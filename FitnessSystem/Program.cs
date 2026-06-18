using FitnessSystem.Data;
using Microsoft.EntityFrameworkCore;
using FitnessSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Добавляем DbContext для работы с базой данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем сессии для авторизации
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Добавляем HttpContextAccessor для получения IP адреса
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

// Маршрутизация — сначала открывается страница входа
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Создаём таблицы если их нет (без миграций)
    dbContext.Database.EnsureCreated();

    // Добавляем администратора если его нет
    if (!dbContext.Users.Any())
    {
        dbContext.Users.Add(new User
        {
            Username = "admin",
            Password = "admin123",
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.Now
        });
        dbContext.SaveChanges();
    }

    // Заполняем базу тестовыми данными
    DbInitializer.Initialize(dbContext);
}

app.Run();
