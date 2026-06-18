using FitnessSystem.Data;
using Microsoft.EntityFrameworkCore;
using FitnessSystem.Models;

// Это ОБЯЗАТЕЛЬНО должно быть первой строкой — до всего остального
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();

    if (!dbContext.Users.Any())
    {
        dbContext.Users.Add(new User
        {
            Username = "admin",
            Password = "admin123",
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });
        dbContext.SaveChanges();
    }

    DbInitializer.Initialize(dbContext);
}

app.Run();
