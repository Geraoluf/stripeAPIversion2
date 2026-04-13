using Microsoft.EntityFrameworkCore;
using Stripe;
using stripeAPI.Data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// 🔑 Stripe key
var stripeKey =
    builder.Configuration["STRIPE_SECRET_KEY"]
    ?? builder.Configuration["Stripe:SecretKey"];

if (string.IsNullOrEmpty(stripeKey))
{
    throw new Exception("Stripe key missing");
}

StripeConfiguration.ApiKey = stripeKey;

// 📁 Data mappe (Azure vs lokal)
var dataDir = builder.Environment.IsDevelopment()
    ? Directory.GetCurrentDirectory()
    : "D:\\home\\data";

if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}

// 🗄️ SQLite database path
var dbPath = builder.Environment.IsDevelopment()
    ? "Data Source=orders.db"
    : "Data Source=D:\\home\\data\\orders.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(dbPath));

// 👇 VIGTIG: MVC + API
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ❗ Error handling i production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// 📦 Static files (wwwroot)
app.UseStaticFiles();

// 🔀 Routing (KRÆVET for MVC)
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

// 🌐 MVC routes (views)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔌 API routes (fx /api/webhook)
app.MapControllers();

// 🗄️ Ensure database exists
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();