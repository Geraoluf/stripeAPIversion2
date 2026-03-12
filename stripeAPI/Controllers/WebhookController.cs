using Microsoft.AspNetCore.Mvc;
using Stripe;
using stripeAPI.Data;
using stripeAPI.Models;

namespace stripeAPI.Controllers
{
    [Route("api/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly AppDbContext _db;
        public WebhookController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Event stripeEvent = EventUtility.ParseEvent(json);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                var ordre = new CustomerOrder
                {
                    Email = session.CustomerDetails.Email,
                    ProductName = "Test Produkt",
                    Quantity = 1,
                    Amount = (decimal)(session.AmountTotal / 100m),
                    Status = "paid",
                    StripeSessionId = session.Id,
                    CreatedAt = DateTime.UtcNow
                };

                _db.CustomerOrders.Add(ordre);
                await _db.SaveChangesAsync();

                Console.WriteLine($"Ny kunde gemt: {ordre.Email}");
            }

            return Ok();
        }
    }
}