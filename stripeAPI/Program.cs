using Microsoft.EntityFrameworkCore;
using Stripe;
using stripeAPI.Data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Stripe key fra Azure environment variable eller local development config
var stripeKey =
    builder.Configuration["STRIPE_SECRET_KEY"]
    ?? builder.Configuration["Stripe:SecretKey"];

// Stop app hvis key mangler (god sikkerhed)
if (string.IsNullOrEmpty(stripeKey))
{
    throw new Exception("Stripe key missing");
}

StripeConfiguration.ApiKey = stripeKey;

// Ensure Azure persistent storage folder exists
var dataDir = builder.Environment.IsDevelopment()
    ? Directory.GetCurrentDirectory()
    : "D:\\home\\data";

if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}

// SQLite database (local dev vs Azure)
var dbPath = builder.Environment.IsDevelopment()
    ? "Data Source=orders.db"
    : "Data Source=D:\\home\\data\\orders.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(dbPath));

// Add services
builder.Services.AddControllers();

var app = builder.Build();

// Exception handler for production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

// Static files (HTML frontend)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure database gets created automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();