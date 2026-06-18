using FitnessSystem.Data;
using Microsoft.EntityFrameworkCore;
using FitnessSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Äîáàâëÿåì DbContext äëÿ ðàáîòû ñ áàçîé äàííûõ
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Äîáàâëÿåì ñåññèè äëÿ àâòîðèçàöèè
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Äîáàâëÿåì HttpContextAccessor äëÿ ïîëó÷åíèÿ IP àäðåñà
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

// Èçìåíÿåì ìàðøðóòèçàöèþ òàê, ÷òîáû ñíà÷àëà îòêðûâàëàñü ñòðàíèöà âõîäà
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // Áûëî Home/Index, òåïåðü Account/Login

// Èíèöèàëèçàöèÿ áàçû äàííûõ íà÷àëüíûìè äàííûìè
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ïðèìåíÿåì âñå îæèäàþùèå ìèãðàöèè
    dbContext.Database.Migrate();

    // Äîáàâëÿåì òåñòîâîãî àäìèíèñòðàòîðà, åñëè åãî íåò
    if (!dbContext.Users.Any())
    {
        dbContext.Users.Add(new User
        {
            Username = "admin",
            Password = "admin123", // Â ðåàëüíîì ïðîåêòå íóæíî õåøèðîâàòü!
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.Now
        });
        dbContext.SaveChanges();
    }
}

app.Run();
