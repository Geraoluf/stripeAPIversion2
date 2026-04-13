using Microsoft.EntityFrameworkCore;
using Stripe;
using stripeAPI.Data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);


var stripeKey =
    builder.Configuration["STRIPE_SECRET_KEY"]
    ?? builder.Configuration["Stripe:SecretKey"];

if (string.IsNullOrEmpty(stripeKey))
{
    throw new Exception("Stripe key missing");
}

StripeConfiguration.ApiKey = stripeKey;


var dataDir = builder.Environment.IsDevelopment()
    ? Directory.GetCurrentDirectory()
    : "D:\\home\\data";

if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}


var dbPath = builder.Environment.IsDevelopment()
    ? "Data Source=orders.db"
    : "Data Source=D:\\home\\data\\orders.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(dbPath));


builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapGet("/", () => Results.Redirect("/Home/Index"));

// 🌐 MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔌 API routes
app.MapControllers();

// 🗄️ Ensure database exists
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();